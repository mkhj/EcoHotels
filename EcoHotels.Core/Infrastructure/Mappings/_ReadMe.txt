NHibernate semantics:

List: 
Ordered collection of entities, duplicate allowed. Use a .net IList in code. 
The index column will need to be mapped in NHibernate.

Set: 
Unordered collection of unique entities, duplicates not allowed. Use Iesi.Collection.ISet in code. 
It is important to override GetHashCode and Equals to indicate the business definition of duplicate. 
Can be sorted by defining a orderby or by defining a comparer resulting in a SortedSet result.

Bag (Multisets): 
Unordered list of entities, duplicates allowed. Use a .net IList in code. 
The index column of the list is not mapped and not honored by NHibernate.