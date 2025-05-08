using System.Collections.Generic;
using System.Linq;

public class SearchByName : ISearchStrategy
{
    public List<Product> Search(List<Product> products, string keyword)
    {
        return products.Where(p => p.Name.ToLower().Contains(keyword.ToLower())).ToList();
    }
}
