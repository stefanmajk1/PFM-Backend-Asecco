using AutoMapper;
using PFMBackend.Database.Entities;
using PFMBackend.Mapping.Entities;
using PFMBackend.Models.Category;
using PFMBackend.Models.Transaction;
using PFMBackend.Models.Transaction.Enums;
using System.Text.RegularExpressions;
using TinyCsvParser.Mapping;

namespace PFMBackend.Mapping
{
    //definisemo AutoMapper profile i konfiguraciju za mapiranje izmedju razlicitih kalsa entiteta
    public class AutoMapperProfile:Profile //iz biblioteke AutoMapper
    {
        public AutoMapperProfile()
        {
            //definisemo pravila mapiranja
            CreateMap<CsvMappingResult<TransactionCsvEntity>, Transaction>()
                .ForMember(d => d.Id, mo => mo.MapFrom(s => s.Result.Id))
                .ForMember(d => d.BeneficiaryName, mo => mo.MapFrom(s => s.Result.BeneficiaryName))
                .ForMember(d => d.Date, mo => mo.MapFrom(s => DateTimeParseUTC(s.Result.Date)))
                .ForMember(d => d.Directions, mo => mo.MapFrom(s => Enum.Parse(typeof(DirectionsEnum), s.Result.Direction, true)))
                .ForMember(d => d.Amount, mo => mo.MapFrom(s => double.Parse(Regex.Match(s.Result.Amount, @"\d+.\d+").Value)))
                .ForMember(d => d.Description, mo => mo.MapFrom(s => s.Result.Description))
                .ForMember(d => d.Currency, mo => mo.MapFrom(s => s.Result.Currency))
                .ForMember(d => d.Mcc, mo => mo.MapFrom(s => s.Result.Mcc))
                .ForMember(d => d.Kind, mo => mo.MapFrom(s => Enum.Parse(typeof(TransactionKindsEnum), s.Result.Kind, true)));

            CreateMap<Transaction, TransactionEntity>();//iz Transaction u TransactionEntity
            CreateMap<TransactionEntity, Transaction>();//iz TransactionEntity u Transaction
            CreateMap<TransactionPagedList<TransactionEntity>, TransactionPagedList<Transaction>>();//iz Transaction u TransactionEntity

            CreateMap<CsvMappingResult<CategoryCsv>, Category>()
                .ForMember(c => c.Code, mo => mo.MapFrom(cs => cs.Result.Code))
                .ForMember(c => c.ParentCode, mo => mo.MapFrom(cs => cs.Result.ParentCode))
                .ForMember(c => c.Name, mo => mo.MapFrom(cs => cs.Result.Name));

            CreateMap<Category, CategoryEntity>();
            CreateMap<CategoryEntity, Category>();
            CreateMap<CategoryList<CategoryEntity>, CategoryList<Category>>();

            CreateMap<SingleCategorySplit, SplitEntity>();
            CreateMap<SplitEntity, SingleCategorySplit>();
            CreateMap<TransactionEntity, TransactionWithSplits>();
            CreateMap<TransactionPagedList<TransactionEntity>, TransactionPagedList<TransactionWithSplits>>();
        }

        private DateTime DateTimeParseUTC(string dateTime)
        {
            DateTime dt = DateTime.Parse(dateTime);

            return dt.ToUniversalTime();
        }
    }
}
