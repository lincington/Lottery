using PaddleOCRSharp;
using System.Drawing;

namespace IndustrialTools.Lib
{
    public class PaddleOCRSharpHelper
    {
       /// <summary>
       /// Initializes a new instance of the PaddleOCRSharpHelper class.
       /// </summary>
        public PaddleOCRSharpHelper()
        {
        }
        private PaddleOCREngine engine;
        OCRModelConfig? config = null;
        public string GetPaddleOCREngine(string path)
        {
               //OCR参数
               OCRParameter oCRParameter = new OCRParameter();
               oCRParameter.cpu_math_library_num_threads = 10;//预测并发线程数
               oCRParameter.enable_mkldnn = true;//web部署该值建议设置为0,否则出错，内存如果使用很大，建议该值也设置为0.
               oCRParameter.cls = false; //是否执行文字方向分类；默认false
               oCRParameter.det = true;//是否开启方向检测，用于检测识别180旋转
               oCRParameter.use_angle_cls = false;//是否开启方向检测，用于检测识别180旋转
               oCRParameter.det_db_score_mode = true;//是否使用多段线，即文字区域是用多段线还是用矩形，
               oCRParameter.max_side_len = 1500;
               oCRParameter.rec_img_h = 48;
               oCRParameter.rec_img_w = 320;
               oCRParameter.det_db_thresh = 0.3f;
               oCRParameter.det_db_box_thresh = 0.618f;
            engine = new PaddleOCREngine(config, oCRParameter);
            StructureModelConfig? structureModelConfig = null;
            StructureParameter structureParameter = new StructureParameter();
            var   structengine = new PaddleStructureEngine(structureModelConfig, structureParameter);

             Bitmap defImage = new  Bitmap (path);
              OCRResult ocrResult = engine.DetectText(defImage);
             return ocrResult.Text;          
        }
      }
}
