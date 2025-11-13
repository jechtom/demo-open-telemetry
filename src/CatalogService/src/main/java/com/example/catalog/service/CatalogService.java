package com.example.catalog.service;

import com.example.catalog.model.Product;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;

public class CatalogService {
    private static final List<Product> PRODUCTS = Arrays.asList(
        new Product("1", "Laptop", "High-performance laptop for work and gaming", 1299.99, "Electronics", 15),
        new Product("2", "Smartphone", "Latest model smartphone with advanced camera", 799.99, "Electronics", 25),
        new Product("3", "Coffee Maker", "Automatic drip coffee maker with programmable timer", 89.99, "Appliances", 30),
        new Product("4", "Running Shoes", "Lightweight running shoes with excellent cushioning", 129.99, "Sports", 40),
        new Product("5", "Office Chair", "Ergonomic office chair with lumbar support", 249.99, "Furniture", 12),
        new Product("6", "Wireless Headphones", "Noise-canceling wireless headphones", 199.99, "Electronics", 20),
        new Product("7", "Water Bottle", "Insulated stainless steel water bottle", 24.99, "Sports", 50),
        new Product("8", "Desk Lamp", "LED desk lamp with adjustable brightness", 45.99, "Furniture", 18),
        new Product("9", "Bluetooth Speaker", "Portable Bluetooth speaker with deep bass", 79.99, "Electronics", 22),
        new Product("10", "Backpack", "Durable laptop backpack with multiple compartments", 59.99, "Accessories", 35)
    );

    public List<Product> getAllProducts() {
        return PRODUCTS;
    }

    public Optional<Product> getProductById(String id) {
        return PRODUCTS.stream()
                .filter(product -> product.getId().equals(id))
                .findFirst();
    }

    public List<Product> getProductsByCategory(String category) {
        return PRODUCTS.stream()
                .filter(product -> product.getCategory().equalsIgnoreCase(category))
                .toList();
    }
}
