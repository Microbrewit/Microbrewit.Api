using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class FermentableSupplierResolver : ValueResolver<FermentableDto, int?>
    {
        protected override int? ResolveCore(FermentableDto source)
        {
//             using (var context = new MicrobrewitContext())
//             {
//                 Supplier supplier = null;
//                 if (source.Supplier != null)
//                 {
//                     supplier = context.Suppliers.SingleOrDefault(s => s.SupplierId == source.Supplier.Id || s.Name.Equals(source.Supplier.Name));
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
//                 return supplier.SupplierId;
//                 }
//                 else
//                 {
//                     return null;
//                 }
//             }
            return 0;

        }
    }
}