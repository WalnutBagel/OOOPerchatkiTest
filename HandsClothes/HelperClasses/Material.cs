using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsClothes.EFData
{
    public partial class Material
    {
        public string GetTypeAndName { get => $"Тип материала: {MaterialType.MaterialTypeName} | Наименование материала:  {MaterialName}"; }

        public string GetMinCount { get => $"Минимальное количество: {MinQuanity} шт."; }

        public string GetCount { get => $"Остаток: {QuanityInStock} шт."; }
        public string GetColor
        {
            get
            {
                if (QuanityInStock <= MinQuanity)
                {
                    return "#f19292";
                }
                else if (QuanityInStock <= MinQuanity * 3)
                {
                    return "#ffba01";
                }
                else
                {
                    return "#fff";
                }
            }
        }
        public string GetImage
        {
            get
            {
                if (string.IsNullOrEmpty(PhotoPath))
                {
                    return $"/Resourses/nullValuePic.png";
                }
                else
                {
                    return $"/Resourses/PhotoMaterials{PhotoPath}";
                }
            }
        }
    }
}
