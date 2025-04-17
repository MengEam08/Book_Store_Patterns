using System.Collections.Generic;

public interface ISearchStrategy
{
    List<Product> Search(List<Product> products, string keyword);
}
