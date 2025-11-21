//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Face;
//using Emgu.CV.Structure;
//using Emgu.CV.Util;

//namespace Billiard.BLL.Services.FaceRecognition
//{
//    /// <summary>
//    /// Service để nhận diện khuôn mặt sử dụng EmguCV (OpenCV wrapper cho .NET)
//    /// Cài đặt: Install-Package Emgu.CV
//    /// </summary>
//    public class FaceRecognitionService
//    {
//        private readonly string _faceDataPath;
//        private readonly string _haarCascadePath;
//        private CascadeClassifier _faceCascade;
//        private EigenFaceRecognizer _faceRecognizer;
//        private List<FaceTrainingData> _trainingData;

//        public FaceRecognitionService(string faceDataPath = "FaceData")
//        {
//            _faceDataPath = faceDataPath;

//            // Đường dẫn tới Haar Cascade XML file (đi kèm với EmguCV)
//            _haarCascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
//                "haarcascade_frontalface_default.xml");

//            InitializeService();
//        }

//        private void InitializeService()
//        {
//            // Tạo thư mục lưu dữ liệu nếu chưa tồn tại
//            if (!Directory.Exists(_faceDataPath))
//            {
//                Directory.CreateDirectory(_faceDataPath);
//            }

//            // Khởi tạo face detector
//            if (File.Exists(_haarCascadePath))
//            {
//                _faceCascade = new CascadeClassifier(_haarCascadePath);
//            }
//            else
//            {
//                throw new FileNotFoundException(
//                    "Không tìm thấy file haarcascade_frontalface_default.xml. " +
//                    "Vui lòng copy file từ EmguCV installation folder.");
//            }

//            // Load training data nếu có
//            LoadTrainingData();
//        }

//        #region Face Detection

//        /// <summary>
//        /// Phát hiện khuôn mặt trong ảnh
//        /// </summary>
//        public Rectangle[] DetectFaces(Image<Bgr, byte> image)
//        {
//            var grayImage = image.Convert<Gray, byte>();
//            var faces = _faceCascade.DetectMultiScale(
//                grayImage,
//                scaleFactor: 1.1,
//                minNeighbors: 3,
//                minSize: new Size(50, 50)
//            );

//            return faces;
//        }

//        /// <summary>
//        /// Phát hiện khuôn mặt từ file ảnh
//        /// </summary>
//        public Rectangle[] DetectFacesFromFile(string imagePath)
//        {
//            using (var image = new Image<Bgr, byte>(imagePath))
//            {
//                return DetectFaces(image);
//            }
//        }

//        /// <summary>
//        /// Phát hiện khuôn mặt từ byte array
//        /// </summary>
//        public Rectangle[] DetectFacesFromBytes(byte[] imageBytes)
//        {
//            using (var ms = new MemoryStream(imageBytes))
//            using (var bitmap = new Bitmap(ms))
//            using (var image = bitmap.ToImage<Bgr, byte>())
//            {
//                return DetectFaces(image);
//            }
//        }

//        #endregion

//        #region Face Training

//        /// <summary>
//        /// Đăng ký khuôn mặt mới cho nhân viên
//        /// </summary>
//        public bool RegisterFace(int maNV, string tenNV, byte[] imageBytes)
//        {
//            try
//            {
//                using (var ms = new MemoryStream(imageBytes))
//                using (var bitmap = new Bitmap(ms))
//                using (var image = bitmap.ToImage<Bgr, byte>())
//                {
//                    // Detect face
//                    var faces = DetectFaces(image);

//                    if (faces.Length == 0)
//                    {
//                        throw new Exception("Không phát hiện được khuôn mặt trong ảnh");
//                    }

//                    if (faces.Length > 1)
//                    {
//                        throw new Exception("Phát hiện nhiều hơn 1 khuôn mặt. Vui lòng chụp ảnh riêng lẻ");
//                    }

//                    // Crop và normalize face
//                    var faceRect = faces[0];
//                    var grayImage = image.Convert<Gray, byte>();
//                    var faceImage = grayImage.Copy(faceRect).Resize(100, 100, Inter.Cubic);

//                    // Lưu ảnh khuôn mặt
//                    var employeeFolder = Path.Combine(_faceDataPath, $"NV_{maNV}");
//                    if (!Directory.Exists(employeeFolder))
//                    {
//                        Directory.CreateDirectory(employeeFolder);
//                    }

//                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
//                    var facePath = Path.Combine(employeeFolder, $"face_{timestamp}.jpg");
//                    faceImage.Save(facePath);

//                    // Thêm vào training data
//                    if (_trainingData == null)
//                    {
//                        _trainingData = new List<FaceTrainingData>();
//                    }

//                    _trainingData.Add(new FaceTrainingData
//                    {
//                        MaNV = maNV,
//                        TenNV = tenNV,
//                        ImagePath = facePath,
//                        FaceImage = faceImage.Clone()
//                    });

//                    // Retrain model
//                    TrainRecognizer();

//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Lỗi đăng ký khuôn mặt: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Load dữ liệu training từ thư mục
//        /// </summary>
//        private void LoadTrainingData()
//        {
//            _trainingData = new List<FaceTrainingData>();

//            if (!Directory.Exists(_faceDataPath))
//                return;

//            var employeeFolders = Directory.GetDirectories(_faceDataPath);

//            foreach (var folder in employeeFolders)
//            {
//                var folderName = Path.GetFileName(folder);
//                if (!folderName.StartsWith("NV_"))
//                    continue;

//                if (!int.TryParse(folderName.Replace("NV_", ""), out int maNV))
//                    continue;

//                var faceFiles = Directory.GetFiles(folder, "*.jpg")
//                    .Concat(Directory.GetFiles(folder, "*.png"))
//                    .ToArray();

//                foreach (var faceFile in faceFiles)
//                {
//                    try
//                    {
//                        var faceImage = new Image<Gray, byte>(faceFile)
//                            .Resize(100, 100, Inter.Cubic);

//                        _trainingData.Add(new FaceTrainingData
//                        {
//                            MaNV = maNV,
//                            ImagePath = faceFile,
//                            FaceImage = faceImage
//                        });
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine($"Lỗi load ảnh {faceFile}: {ex.Message}");
//                    }
//                }
//            }

//            if (_trainingData.Count > 0)
//            {
//                TrainRecognizer();
//            }
//        }

//        /// <summary>
//        /// Train face recognizer với dữ liệu hiện có
//        /// </summary>
//        private void TrainRecognizer()
//        {
//            if (_trainingData == null || _trainingData.Count == 0)
//                return;

//            try
//            {
//                var images = new VectorOfMat();
//                var labels = new VectorOfInt();

//                foreach (var data in _trainingData)
//                {
//                    images.Push(data.FaceImage.Mat);
//                    labels.Push(new[] { data.MaNV });
//                }

//                _faceRecognizer = new EigenFaceRecognizer(numComponents: 80, threshold: 5000);
//                _faceRecognizer.Train(images, labels);

//                Console.WriteLine($"✅ Đã train {_trainingData.Count} khuôn mặt");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Lỗi train model: {ex.Message}");
//            }
//        }

//        #endregion

//        #region Face Recognition

//        /// <summary>
//        /// Nhận diện khuôn mặt từ ảnh
//        /// </summary>
//        public FaceRecognitionResult RecognizeFace(byte[] imageBytes)
//        {
//            if (_faceRecognizer == null || _trainingData == null || _trainingData.Count == 0)
//            {
//                return new FaceRecognitionResult
//                {
//                    Success = false,
//                    Message = "Chưa có dữ liệu training. Vui lòng đăng ký khuôn mặt trước."
//                };
//            }

//            try
//            {
//                using (var ms = new MemoryStream(imageBytes))
//                using (var bitmap = new Bitmap(ms))
//                using (var image = bitmap.ToImage<Bgr, byte>())
//                {
//                    // Detect faces
//                    var faces = DetectFaces(image);

//                    if (faces.Length == 0)
//                    {
//                        return new FaceRecognitionResult
//                        {
//                            Success = false,
//                            Message = "Không phát hiện được khuôn mặt"
//                        };
//                    }

//                    if (faces.Length > 1)
//                    {
//                        return new FaceRecognitionResult
//                        {
//                            Success = false,
//                            Message = "Phát hiện nhiều khuôn mặt. Vui lòng chỉ 1 người trong khung hình"
//                        };
//                    }

//                    // Extract and normalize face
//                    var faceRect = faces[0];
//                    var grayImage = image.Convert<Gray, byte>();
//                    var faceImage = grayImage.Copy(faceRect).Resize(100, 100, Inter.Cubic);

//                    // Recognize
//                    var result = _faceRecognizer.Predict(faceImage);
//                    int predictedLabel = result.Label;
//                    double confidence = result.Distance;

//                    // Threshold để xác định có match hay không
//                    const double CONFIDENCE_THRESHOLD = 5000;

//                    if (confidence < CONFIDENCE_THRESHOLD)
//                    {
//                        var matchedEmployee = _trainingData
//                            .FirstOrDefault(x => x.MaNV == predictedLabel);

//                        if (matchedEmployee != null)
//                        {
//                            return new FaceRecognitionResult
//                            {
//                                Success = true,
//                                MaNV = predictedLabel,
//                                TenNV = matchedEmployee.TenNV,
//                                Confidence = confidence,
//                                Message = "Nhận diện thành công"
//                            };
//                        }
//                    }

//                    return new FaceRecognitionResult
//                    {
//                        Success = false,
//                        Message = "Không nhận diện được khuôn mặt này",
//                        Confidence = confidence
//                    };
//                }
//            }
//            catch (Exception ex)
//            {
//                return new FaceRecognitionResult
//                {
//                    Success = false,
//                    Message = $"Lỗi nhận diện: {ex.Message}"
//                };
//            }
//        }

//        #endregion

//        #region Helper Methods

//        /// <summary>
//        /// Xóa dữ liệu khuôn mặt của nhân viên
//        /// </summary>
//        public bool DeleteEmployeeFaceData(int maNV)
//        {
//            try
//            {
//                var employeeFolder = Path.Combine(_faceDataPath, $"NV_{maNV}");

//                if (Directory.Exists(employeeFolder))
//                {
//                    Directory.Delete(employeeFolder, true);
//                }

//                // Remove from training data
//                _trainingData?.RemoveAll(x => x.MaNV == maNV);

//                // Retrain if data exists
//                if (_trainingData?.Count > 0)
//                {
//                    TrainRecognizer();
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi xóa dữ liệu: {ex.Message}");
//                return false;
//            }
//        }

//        /// <summary>
//        /// Lấy số lượng ảnh đã đăng ký của nhân viên
//        /// </summary>
//        public int GetRegisteredFaceCount(int maNV)
//        {
//            return _trainingData?.Count(x => x.MaNV == maNV) ?? 0;
//        }

//        /// <summary>
//        /// Kiểm tra xem nhân viên đã đăng ký Face ID chưa
//        /// </summary>
//        public bool HasRegisteredFace(int maNV)
//        {
//            return GetRegisteredFaceCount(maNV) > 0;
//        }

//        #endregion
//    }

//    #region Supporting Classes

//    public class FaceTrainingData
//    {
//        public int MaNV { get; set; }
//        public string TenNV { get; set; }
//        public string ImagePath { get; set; }
//        public Image<Gray, byte> FaceImage { get; set; }
//    }

//    public class FaceRecognitionResult
//    {
//        public bool Success { get; set; }
//        public int MaNV { get; set; }
//        public string TenNV { get; set; }
//        public double Confidence { get; set; }
//        public string Message { get; set; }
//    }

//    #endregion
//}

///// <summary>
///// Extension methods cho Image conversion
///// </summary>
//public static class BitmapExtensions
//{
//    public static Image<Bgr, byte> ToEmguImage(this Bitmap bitmap)
//    {
//        using (var ms = new MemoryStream())
//        {
//            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//            ms.Position = 0;
//            return new Image<Bgr, byte>(bitmap);
//        }
//    }
//}