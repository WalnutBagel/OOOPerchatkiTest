using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsClothes.EFData
{
    public partial class Material
    {

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
