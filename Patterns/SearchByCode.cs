using System.Collections.Generic;
using System.Linq;

public class SearchByCode : ISearchStrategy
{
    public List<Product> Search(List<Product> products, string keyword)
    {
        return products.Where(p => p.Code.ToLower().Contains(keyword.ToLower())).ToList();
    }
}
