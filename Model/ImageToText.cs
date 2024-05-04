using Patagames.Ocr.Enums;
using Patagames.Ocr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Size = System.Drawing.Size;

namespace VirtualEyes.Model
{
    public class ImageToText
    {
        private OcrApi imgReader;
        private Languages activeLanguage = Languages.English;
        public ImageToText() 
        {
            imgReader = OcrApi.Create();
            imgReader.Init(activeLanguage);
        }

        public string ReadScreenText(Rect screenarea,Languages language = Languages.English)
        {
            Bitmap ReadTarget = new Bitmap((int)screenarea.Width, (int)screenarea.Height);
            string text = string.Empty;

            using (Graphics g = Graphics.FromImage(ReadTarget))
            {
                g.CopyFromScreen(Math.Min((int)screenarea.X, (int)screenarea.X),
                                 Math.Min((int)screenarea.Y, (int)screenarea.Y),
                                 0, 0,
                                 new Size((int)screenarea.Width, (int)screenarea.Height));
            }

            try
            {
                if(activeLanguage != language)
                {
                    imgReader.Init(language);
                    activeLanguage = language;
                }
                text = imgReader.GetTextFromImage(ReadTarget);
                ReadTarget.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Selected area too big, max 500x500");
            }
            return text;
        }
    }
}
