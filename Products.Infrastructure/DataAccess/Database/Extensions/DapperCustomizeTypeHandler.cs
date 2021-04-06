using Dapper;
using Newtonsoft.Json;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Products.Infrastructure.DataAccess.Database.Extensions
{
    public class DapperCustomizeTypeHandler : SqlMapper.TypeHandler<IList<CustomizeValue>>
    {
        public override void SetValue(IDbDataParameter parameter, IList<CustomizeValue> value)
        {
            parameter.Value = value.ToString();
        }

        public override IList<CustomizeValue> Parse(object value)
        {
            return JsonConvert.DeserializeObject<IList<CustomizeValue>>((string)value);
        }
    }
}
