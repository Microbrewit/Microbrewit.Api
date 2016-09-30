using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class YeastSupplierResolver : ValueResolver<YeastDto, int?>
    {
        protected override int? ResolveCore(YeastDto source)
        {
//             using (var context = new MicrobrewitContext())
//             {
//                 if (source.Supplier != null)
//                 {
//                     var supplier = context.Suppliers.SingleOrDefault(s => s.SupplierId == source.Supplier.Id || s.Name.Equals(source.Supplier.Name));
// 
//                     if (supplier == null)
//                     {
//                         supplier = new Supplier()
//                         {
//                             Name = source.Supplier.Name,
//                         };
//                         context.Suppliers.Add(supplier);
//                         context.SaveChanges();
//                     }
//                     return supplier.SupplierId;
//                 }
                if(source?.Supplier?.Id != null)
                {
                    return source.Supplier.Id;
                }
                return null;
            //}
        }
    }
}