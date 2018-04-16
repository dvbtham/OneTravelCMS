using OneTravelApi.Responses;
using OneTravelCMS.Core;
using OneTravelCMS.Core.Extensions.HttpClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneTravelCMS.Models
{
    public class FunctionWithRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FunctionCode { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
        public int Parent { get; set; }

        public IList<FunctionWithRole> ChildItems { get; set; } = new List<FunctionWithRole>();

    }

    public class FunctionHelper
    {
        public async Task<List<FunctionWithRole>> BuildLeftMenu(IList<string> roleCodes, string areaCode, string email)
        {
            var response = await HttpRequestFactory.Get(Constants.BaseAdminApiUrl + "user/areas/" + email);
            var outputModel = response.ContentAsType<SingleModelResponse<UserEx>>();
            
            var mRoles = new List<MyRole>();
            var myFunctions = new List<FunctionWithRole>();
            var childs = new List<FunctionWithRole>();

            foreach (var rc in roleCodes)
            {
                if (!string.IsNullOrEmpty(rc))
                {
                    mRoles.Add(outputModel.Model.MyRoles.FirstOrDefault(x => x.RoleCode == rc));
                }
            }

            var areas = mRoles.SelectMany(x => x.MyAreas.Where(a => a.AreaCode == areaCode));

            foreach (var myArea in areas)
            {
                foreach (var function in myArea.MyFunctions)
                {
                    foreach (var childItem in function.ChildItems)
                    {
                        if (childs.Any(x => x.Id == childItem.Id)) continue;

                        childs.Add(childItem);
                    }

                    if (myFunctions.Any(x => x.Id == function.Id)) continue;
                    myFunctions.Add(function);
                }
            }

            foreach (var myFunction in myFunctions)
            {
                myFunction.ChildItems = childs.Where(x => x.Parent == myFunction.Id).ToList();
            }
            return myFunctions;
        }
    }

    public class UserEx
    {
        public IList<MyRole> MyRoles = new List<MyRole>();
    }

    public class MyRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleCode { get; set; }
        public IList<MyArea> MyAreas { get; set; } = new List<MyArea>();
        public IList<KeyValuePairResource> AllAreas { get; set; } = new List<KeyValuePairResource>();
    }

    public class MyArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AreaCode { get; set; }
        public IList<FunctionWithRole> MyFunctions { get; set; } = new List<FunctionWithRole>();
        public IList<KeyValuePairResource> AllFunctions { get; set; } = new List<KeyValuePairResource>();
    }
}