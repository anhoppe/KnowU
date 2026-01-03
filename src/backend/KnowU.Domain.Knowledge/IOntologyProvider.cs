using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

internal interface IOntologyProvider
{
    string GetOntologyPromptSection();
    string GetJsonSchemaExample();
    EntityClass? FindClass(string classId);
    PredicateProperty? FindPredicate(string predicateId);
}
