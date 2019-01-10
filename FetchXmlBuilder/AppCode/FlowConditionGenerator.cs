using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinteros.Xrm.FetchXmlBuilder.AppCode
{
    public class FlowConditionGenerator
    {
        public static string GetFlowCondition(FetchType fetch, string organizationServiceUrl, FetchXmlBuilder sender)
        {
            if (sender.Service == null)
            {
                throw new Exception("Must have an active connection to CRM to compose Flow conditions.");
            }
            var url = organizationServiceUrl;
            var entity = fetch.Items.Where(i => i is FetchEntityType).FirstOrDefault() as FetchEntityType;
            
            if (entity == null)
            {
                throw new Exception("Fetch must contain entity definition");
            }

            var query = "";

            if (entity.Items != null)
            {
                #region Validation
                var linkitems = entity.Items.Where(i => i is FetchLinkEntityType).ToList();

                if (linkitems.Any())
                {
                    foreach (FetchLinkEntityType linkitem in linkitems)
                    {
                        if (linkitem.Items != null)
                        {
                            if (linkitem.Items.Where(i => i is filter).ToList().Count > 0)
                            {
                                throw new Exception(
                                    "Flow conditions generator does not support filters on linked entities");
                            }
                        }
                    }
                }
                #endregion
                
                query = GetFilter(entity, sender);
            }

            return query;
        }

        private static string GetFilter(FetchEntityType entity, FetchXmlBuilder sender)
        {
            var resultList = new StringBuilder("@");
            var filteritems = entity.Items.Where(i => i is filter && ((filter)i).Items != null && ((filter)i).Items.Length > 0).ToList();
            if (filteritems.Count > 0)
            {
                foreach (filter filteritem in filteritems)
                {
                    resultList.Append(GetFilter(entity, filteritem, sender));
                }
                var result = resultList.ToString();
                
                return result;
            }
            return "";
        }

        private static string GetFilter(FetchEntityType entity, filter filteritem, FetchXmlBuilder sender)
        {
            var result = "";
            if (filteritem.Items == null || filteritem.Items.Length == 0)
            {
                return "";
            }
            var logical = filteritem.type == filterType.or ? "or" : "and";
            if (filteritem.Items.Length > 0)
            {
                result = $"{logical}(";
            }

            int count = 0;

            foreach (var item in filteritem.Items)
            {
                if (count > 0) result += ",";

                if (item is condition)
                {
                    result += GetCondition(entity, item as condition, sender);
                }
                else if (item is filter)
                {
                    result += GetFilter(entity, item as filter, sender);
                }

                count++;
            }
            
            if (filteritem.Items.Length > 0)
            {
                result += ")";
            }

            return result;
        }

        private static string GetCondition(FetchEntityType entity, condition condition, FetchXmlBuilder sender)
        {
            var result = "";
            string conditiontoAppend = "";
            string attributeName = "";
            string value = "";

            if (!string.IsNullOrEmpty(condition.attribute))
            {
                string stepToApplyConditions = sender.GetFlowStep();
                GetEntityMetadata(entity.name, sender);
                var attrMeta = sender.GetAttribute(entity.name, condition.attribute);
                if (attrMeta == null)
                {
                    throw new Exception($"No metadata for attribute: {entity.name}.{condition.attribute}");
                }
                attributeName = $"{stepToApplyConditions}?['{attrMeta.LogicalName}']";

                #region Helper variables
                string onorafterCondition;
                string beforeCondition;
                string nextdateValue;

                bool isValue = false;
                bool isGuid = false;
                bool isDateTime = false;

                switch (attrMeta.AttributeType)
                {
                    case AttributeTypeCode.Money:
                    case AttributeTypeCode.BigInt:
                    case AttributeTypeCode.Boolean:
                    case AttributeTypeCode.Decimal:
                    case AttributeTypeCode.Double:
                    case AttributeTypeCode.Integer:
                    case AttributeTypeCode.State:
                    case AttributeTypeCode.Status:
                    case AttributeTypeCode.Picklist:
                        isValue = true;
                        break;
                    case AttributeTypeCode.Uniqueidentifier:
                    case AttributeTypeCode.Lookup:
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Owner:
                        isGuid = true;
                        break;
                    case AttributeTypeCode.DateTime:
                        isDateTime = true;
                        break;
                    default:
                        break;
                }
                #endregion

                if (!string.IsNullOrEmpty(condition.value))
                {
                    value = condition.value;
                }

                switch (condition.@operator)
                {
                    case @operator.eq:
                        conditiontoAppend = FlowOperators.equals;
                        break;
                    case @operator.ne:
                        conditiontoAppend = FlowOperators.doesnotequal;
                        break;
                    case @operator.lt:
                        conditiontoAppend = FlowOperators.lessthan;
                        break;
                    case @operator.le:
                        conditiontoAppend = FlowOperators.lessthanOrEqual;
                        break;
                    case @operator.gt:
                        conditiontoAppend = FlowOperators.greaterthan;
                        break;
                    case @operator.ge:
                        conditiontoAppend = FlowOperators.greaterthanOrEqual;
                        break;
                    case @operator.neq:
                        conditiontoAppend = FlowOperators.doesnotequal;
                        break;
                    case @operator.@null:
                        conditiontoAppend = FlowOperators.equals;
                        value = "null";
                        break;
                    case @operator.notnull:
                        conditiontoAppend = FlowOperators.doesnotequal;
                        value = "null";
                        break;
                    case @operator.like:
                        if (!condition.value.StartsWith("%") && !condition.value.EndsWith("%"))
                        {
                            conditiontoAppend = FlowOperators.contains;
                        }
                        else if (!condition.value.StartsWith("%") && condition.value.EndsWith("%"))
                        {
                            conditiontoAppend = FlowOperators.startsWith;
                        }
                        else if (condition.value.StartsWith("%") && !condition.value.EndsWith("%"))
                        {
                            conditiontoAppend = FlowOperators.endswith;
                        }
                        value = condition.value.Replace("%", "");
                        break;
                    case @operator.notlike:
                        if (!condition.value.StartsWith("%") && !condition.value.EndsWith("%"))
                        {
                            conditiontoAppend = FlowOperators.doesnotcontain;
                        }
                        else if (!condition.value.StartsWith("%") && condition.value.EndsWith("%"))
                        {
                            conditiontoAppend = FlowOperators.doesnotstartWith;
                        }
                        else if (condition.value.StartsWith("%") && !condition.value.EndsWith("%"))
                        {
                            conditiontoAppend = FlowOperators.doesnotendwith;
                        }
                        value = condition.value.Replace("%", "");
                        break;
                    case @operator.@in:
                        value = "";
                        conditiontoAppend += "or(";
                        foreach (conditionValue conValue in condition.Items)
                        {
                            if (!conditiontoAppend.Equals("or(")) conditiontoAppend += ",";

                            if (isGuid)
                            {
                                value = $"'{conValue.Value.Replace("{", "").Replace("}", "").ToLower()}'";
                            }
                            else if(isValue)
                            {
                                value = conValue.Value;
                            }
                            else
                            {
                                value = $"'{conValue.Value}'";
                            }

                            conditiontoAppend += FlowOperators.equals.Replace("<attributename>", attributeName)
                                .Replace("<value>", value);
                        }
                        conditiontoAppend += ")";
                        break;
                    case @operator.notin:
                        value = "";
                        conditiontoAppend += "and(";
                        foreach (conditionValue conValue in condition.Items)
                        {
                            if (!conditiontoAppend.Equals("and(")) conditiontoAppend += ",";

                            if (isGuid)
                            {
                                value = $"'{conValue.Value.Replace("{", "").Replace("}", "").ToLower()}'";
                            }
                            else if (isValue)
                            {
                                value = conValue.Value;
                            }
                            else
                            {
                                value = $"'{conValue.Value}'";
                            }

                            conditiontoAppend += FlowOperators.doesnotequal.Replace("<attributename>", attributeName)
                                .Replace("<value>", value);
                        }
                        conditiontoAppend += ")";
                        break;
                    case @operator.beginswith:
                        conditiontoAppend = FlowOperators.startsWith;
                        break;
                    case @operator.notbeginwith:
                        conditiontoAppend = FlowOperators.doesnotstartWith;
                        break;
                    case @operator.endswith:
                        conditiontoAppend = FlowOperators.endswith;
                        break;
                    case @operator.notendwith:
                        conditiontoAppend = FlowOperators.doesnotendwith;
                        break;
                    case @operator.on:
                        onorafterCondition = FlowOperators.greaterthanOrEqual;
                        beforeCondition = FlowOperators.lessthan;

                        value = $"'{GetDatePartFromString(condition.value)}'";
                        onorafterCondition = onorafterCondition.Replace("<value>", value);

                        nextdateValue = $"'{GetNextDatePartFromString(condition.value)}'";
                        beforeCondition = beforeCondition.Replace("<value>", nextdateValue);

                        conditiontoAppend = $"and({onorafterCondition},{beforeCondition})";
                        break;
                    /*case @operator.noton:
                        onorafterCondition = FlowOperators.greaterthanOrEqual;
                        beforeCondition = FlowOperators.lessthan;

                        value = $"'{GetDatePartFromString(value)}'";
                        beforeCondition = beforeCondition.Replace("<value>", value);

                        nextdateValue = $"'{GetNextDatePartFromString(value)}'";
                        onorafterCondition = onorafterCondition.Replace("<value>", nextdateValue);

                        conditiontoAppend = $"and({beforeCondition},{onorafterCondition})";
                        break;*/
                    case @operator.onorafter:
                        conditiontoAppend = FlowOperators.greaterthanOrEqual;
                        //remove time part from value
                        value = GetDatePartFromString(value);
                        break;
                    case @operator.onorbefore:
                        conditiontoAppend = FlowOperators.lessthanOrEqual;
                        //remove time part from value
                        value = GetDatePartFromString(value);
                        break;
                    default:
                        throw new Exception($"Unsupported Flow condition operator '{condition.@operator}'");
                }

                if (!string.IsNullOrEmpty(value))
                {
                    if (isGuid)
                    {
                        value = $"'{value.Replace("{", "").Replace("}", "").ToLower()}'";
                    }
                    else if(isDateTime)
                    {
                        value = $"'{GetDatePartFromString(value)}'";
                    }
                    else if(!isValue)
                    {
                        value = $"'{value}'";
                    }
                }

                result = conditiontoAppend.Replace("<attributename>", attributeName).Replace("<value>", value);
            }
            return result;
        }

        private static void GetEntityMetadata(string entity, FetchXmlBuilder sender)
        {
            if (sender.NeedToLoadEntity(entity))
            {
                sender.LoadEntityDetails(entity, null, false);
            }
            if (!sender.entities.ContainsKey(entity))
            {
                throw new Exception($"No metadata for entity: {entity}");
            }
        }
        /// <summary>
        /// This method removes the time part from a date time string
        /// </summary>
        /// <param name="value"></param>
        private static string GetDatePartFromString(string value)
        {
            try
            {
                //remove time part from value
                DateTime date = Convert.ToDateTime(value).Date;
                value = date.ToString("yyyy-MM-dd");
                return value;
            }
            catch (Exception e)
            {
                return value;
            }
        }

        /// <summary>
        /// This method removes the time part from a date time string and returns the next day value
        /// </summary>
        /// <param name="value"></param>
        private static string GetNextDatePartFromString(string value)
        {
            try
            {
                //remove time part from value
                DateTime date = Convert.ToDateTime(value).Date.AddDays(1);
                value = date.ToString("yyyy-MM-dd");
                return value;
            }
            catch (Exception e)
            {
                return value;
            }
        }
    }

    public class FlowOperators
    {
        public static string contains = "contains(<attributename>,<value>)";
        public static string doesnotcontain = "not(contains(<attributename>,<value>))";
        public static string equals = "equals(<attributename>,<value>)";
        public static string doesnotequal = "not(equals(<attributename>,<value>))";
        public static string startsWith = "startsWith(<attributename>,<value>)";
        public static string doesnotstartWith = "not(startsWith(<attributename>,<value>))";
        public static string endswith = "endswith(<attributename>,<value>)";
        public static string doesnotendwith = "not(endswith(<attributename>,<value>))";
        public static string greaterthan = "greater(<attributename>,<value>)";
        public static string greaterthanOrEqual = "greaterOrEquals(<attributename>,<value>)";
        public static string lessthan = "less(<attributename>,<value>)";
        public static string lessthanOrEqual = "lessOrEquals(<attributename>,<value>)";
    }
}