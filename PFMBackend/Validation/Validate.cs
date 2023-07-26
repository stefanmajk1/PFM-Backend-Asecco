using PFMBackend.Mapping.Entities;
using PFMBackend.Models.Transaction.Enums;
using PFMBackend.Problem;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using TinyCsvParser.Mapping;

namespace PFMBackend.Validation
{
    public class Validate
    {

        //properties
        public string PropertyName { get; set; }
        public bool Required { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string Pattern { get; set; }
        public bool IsNumber { get; set; }
        public bool IsEnum { get; set; }

        //ovo je genericka metoda, koja prihvata listu objekata tipa T, i vraca ValidationProblem
        public static ValidationProblem ValidateList<T>(List<T> list)
        {
            List<Errors> errors = new List<Errors>();//prazna lista errora

            //punimo listu ako ispunjava uslove
            if (typeof(T) == typeof(CsvMappingResult<TransactionCsvEntity>))
            {
                errors = ValidateCsvMappingResultListTransactions(list as List<CsvMappingResult<TransactionCsvEntity>>);
            }
            else if (typeof(T) == typeof(CsvMappingResult<CategoryCsv>))
            {
                errors = ValidateCsvMappingResultListCategories(list as List<CsvMappingResult<CategoryCsv>>);
            }

            return new ValidationProblem
            {
                Errors = errors
            };
        }

        //ova metoda radi validaciju na listu od 'CsvMappingResult<TransactionCsvEntity>'
        private static List<Errors> ValidateCsvMappingResultListTransactions(List<CsvMappingResult<TransactionCsvEntity>> list)
        {
            List<Errors> errors = new List<Errors>();//prazna lista

            //lista pravila validacija
            List<Validate> validations = new List<Validate>(){
                    {new Validate{PropertyName = "Result.Id", Required = true}},
                    {new Validate{PropertyName = "Result.Date", Required = true}},
                    {new Validate{PropertyName = "Result.Direction", Required = true, IsEnum = true}},
                    {new Validate{PropertyName = "Result.Amount", Required = true, Pattern = @"\d+.\d+", IsNumber=true}},
                    {new Validate{PropertyName = "Result.Currency", Required = true, MinLength = 3, MaxLength = 3}},
                    {new Validate{PropertyName = "Result.Mcc", Required = false, IsEnum = true}},
                    {new Validate{PropertyName = "Result.Kind", Required = true, IsEnum = true}}
                };
            string value;
            DateTime pomDate;
            ErrEnum err;
            double pomDouble;

            foreach (var item in list)//prolazimo kroz listu
            {
                foreach (var property in validations)//prolazimo kroz validacije
                {
                    value = GetPropertyValue(item, property.PropertyName).ToString();
                    if (property.Required)
                    {
                        if (string.IsNullOrEmpty(value.ToString()))
                        {
                            err = ErrEnum.Required;
                            errors.Add(CreateError(SetOutputPropertyName(property.PropertyName.Split('.')[1]), err, GetEnumDescription(err)));
                            break;
                        }
                    }
                    if (property.PropertyName.ToLower().Contains("date"))
                    {
                        if (!DateTime.TryParse(value, out pomDate))
                        {
                            err = ErrEnum.InvalidFormat;
                            errors.Add(CreateError(SetOutputPropertyName(property.PropertyName.Split('.')[1]), err, GetEnumDescription(err)));
                            break;
                        }
                    }
                    if ((property.MinLength > 0 && property.MaxLength > 0))
                    {
                        if (value.Length < property.MinLength)
                        {
                            err = ErrEnum.MinLength;
                            errors.Add(CreateError(SetOutputPropertyName(property.PropertyName.Split('.')[1]), err, GetEnumDescription(err)));
                            break;
                        }
                        else if (value.Length > property.MinLength)
                        {
                            err = ErrEnum.MaxLength;
                            errors.Add(CreateError(SetOutputPropertyName(property.PropertyName.Split('.')[1]), err, GetEnumDescription(err)));
                            break;
                        }
                    }
                    if (property.IsNumber)
                    {
                        if (!double.TryParse(Regex.Match(value, property.Pattern).Value, out pomDouble))
                        {
                            err = ErrEnum.InvalidFormat;
                            errors.Add(CreateError(SetOutputPropertyName(property.PropertyName.Split('.')[1]), err, GetEnumDescription(err)));
                            break;
                        }
                    }
                    if (property.IsEnum && value.Length > 0)
                    {
                        if (!TryParseEnums(value))
                        {
                            err = ErrEnum.UnknownEnum;
                            errors.Add(CreateError(SetOutputPropertyName(property.PropertyName.Split('.')[1]), err, GetEnumDescription(err)));
                            break;
                        }
                    }
                }
            }
            return errors;
        }

        //odgovoran za validaciju liste 'CsvMappingResult<CategoryCsv>'
        private static List<Errors> ValidateCsvMappingResultListCategories(List<CsvMappingResult<CategoryCsv>> list)
        {
            List<Errors> errors = new List<Errors>();//prazna lista, koju cemo napuniti erorima

            List<Validate> validations = new List<Validate>(){//lista validacija
                    {new Validate{PropertyName = "Result.Code", Required = true}},
                    {new Validate{PropertyName = "Result.Name", Required = true}}
                };
            string value;
            ErrEnum err;

            foreach (var item in list)
            {
                foreach (var property in validations)
                {
                    value = GetPropertyValue(item, property.PropertyName).ToString();
                    if (property.Required)
                    {
                        if (string.IsNullOrEmpty(value.ToString()))//ako je vrednost prazna, vratice error
                        {
                            err = ErrEnum.Required;
                            //punimo error listu, i dajemo mu deskripciju
                            errors.Add(CreateError(SetOutputPropertyName(property.PropertyName.Split('.')[1]), err, GetEnumDescription(err)));
                            //izlazimo iz petlja
                            break;
                        }
                    }
                }
            }

            return errors;//vracamo errore, koje smo pronasli
        }
        //pretvaranje naziv svojstva za standarizovan format za oznake greske
        private static string SetOutputPropertyName(string propName)
        {
            string s = $"{char.ToLower(propName[0])}";
            for (int i = 1; i < propName.Length; i++)
            {
                if (char.IsUpper(propName[i]))
                {
                    s += $"-{char.ToLower(propName[i])}";
                }
                else
                {
                    s += propName[i];
                }
            }

            return s;
        }
        //parsira string reprezentaciju vrednosti enumeracije, i vraca true ili false
        private static bool TryParseEnums(string enumeration)
        {
            object en;
            return Enum.TryParse(typeof(DirectionsEnum), enumeration, true, out en) || Enum.TryParse(typeof(MccCodeEnum), enumeration, true, out en)
            || Enum.TryParse(typeof(TransactionKindsEnum), enumeration, true, out en);
        }
        //kreiranje objekata, sa pruzenom oznakom
        private static Errors CreateError(string tag, ErrEnum err, string message)
        {
            return new Errors
            {
                Tag = tag,
                Error = err,
                Message = message
            };
        }

        // preuzimanje opisa vrednosti enumeracije koristeći refleksiju.
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            //Traži 'DescriptionAttribute' povezan sa vrednošću enumeracije kako bi pružila čitljiviji opis
            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        //dobijanje vrednosti svojstva iz datog objekta pomoću refleksije
        private static object GetPropertyValue(object src, string propName)
        {
            if (propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop.GetValue(src, null);
            }
        }
    }
}
