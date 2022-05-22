﻿using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IProductRepository
    {
        Product LoadProductById(int productId, int userId);

        List<ProductLight> LoadProductByCategoryId(int categoryId, int userId);

        Product SaveProduct(Product product);

        Product UpdateProduct(int id, Product product);

        void DeleteProduct(int id);
    }
}
