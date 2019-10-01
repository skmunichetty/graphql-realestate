using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Types
{
    public class EntityInterface : InterfaceGraphType
    {
        public EntityInterface()
        {
            Field<IdGraphType>("id");
            Field<DateGraphType>("created");
            Field<DateGraphType>("edited");
            Field<StringGraphType>("url");
        }
    }
}
