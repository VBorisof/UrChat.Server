using System;
using System.Collections.Generic;
using System.Text;
using UrChat.Forms;
using UrChat.Services.Shared;

namespace UrChat.Services
{
    public class TranslationService
    {
        public static List<string> RandomUrs = new List<string>
        {
            "Урррруруррррурурр",
            "Уррррурурурр",
            "Уррррурурр",
            "Уррррурурррмммрурурр",
            "Урррруруррррурурммр",
            "Урмммммррруруррррурурр",
            "Уррррурумммммррррурурр",
            "Урррруруррррммммммммурурр",
        };
        
        public static Dictionary<char, string> EnglishAlphabetToUr = new Dictionary<char, string>
        {
            { 'A', "У"         },
            { 'B', "Р"         },
            { 'C', "Ру"        },
            { 'D', "Рру"       },
            { 'E', "Ур"        },
            { 'F', "Ррур"      },
            { 'G', "Рррр"      },
            { 'H', "Рррру"     },
            { 'I', "Урр"       },
            { 'J', "Ррррр"     },
            { 'K', "Ррррру"    },
            { 'L', "Мр"        },
            { 'M', "Мрр"       },
            { 'N', "Мрру"      },
            { 'O', "Уррур"     },
            { 'P', "Уррррр"    },
            { 'Q', "Рруррру"   },
            { 'R', "Ррррурр"   },
            { 'S', "Рррррру"   },
            { 'T', "Ррррррр"   },
            { 'U', "Уррррррр"  },
            { 'V', "Ррррррррр" },
            { 'W', "Рррррррру" },
            { 'X', "Ррррррурр" },
            { 'Y', "Урррррурр" },
            { 'Z', "Ррррррурр" }
        };
        
        public ServiceOperationResult<string> TranslateMessageToUr(string message)
        {
            var random = new Random();
            var builder = new StringBuilder();
            foreach (var @char in message)
            {
                if (!EnglishAlphabetToUr.ContainsKey(char.ToUpper(@char)))
                {
                    builder.Append(RandomUrs[random.Next(0, RandomUrs.Count - 1)]);
                }
                else builder.Append(EnglishAlphabetToUr[char.ToUpper(@char)]);
            }

            return ServiceOperationResult.Ok(builder.ToString());
        }
    }
}