package com.example.catalog.handler;

import com.example.catalog.model.Product;
import com.example.catalog.service.CatalogService;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;

import java.io.IOException;
import java.io.OutputStream;
import java.net.URI;
import java.util.List;
import java.util.Optional;

public class CatalogHandler implements HttpHandler {
    private final CatalogService catalogService;
    private final ObjectMapper objectMapper;

    public CatalogHandler() {
        this.catalogService = new CatalogService();
        this.objectMapper = new ObjectMapper();
    }

    @Override
    public void handle(HttpExchange exchange) throws IOException {
        // Add CORS headers
        exchange.getResponseHeaders().add("Access-Control-Allow-Origin", "*");
        exchange.getResponseHeaders().add("Access-Control-Allow-Methods", "GET, OPTIONS");
        exchange.getResponseHeaders().add("Access-Control-Allow-Headers", "Content-Type");
        exchange.getResponseHeaders().add("Content-Type", "application/json");

        String method = exchange.getRequestMethod();
        
        if ("OPTIONS".equals(method)) {
            // Handle preflight request
            exchange.sendResponseHeaders(200, 0);
            exchange.close();
            return;
        }

        if (!"GET".equals(method)) {
            sendResponse(exchange, 405, "{\"error\":\"Method not allowed\"}");
            return;
        }

        try {
            URI uri = exchange.getRequestURI();
            String path = uri.getPath();
            String query = uri.getQuery();

            if ("/api/products".equals(path)) {
                handleProductsRequest(exchange, query);
            } else if (path.startsWith("/api/products/")) {
                handleProductByIdRequest(exchange, path);
            } else if ("/health".equals(path)) {
                sendResponse(exchange, 200, "{\"status\":\"healthy\"}");
            } else {
                sendResponse(exchange, 404, "{\"error\":\"Not found\"}");
            }
        } catch (Exception e) {
            sendResponse(exchange, 500, "{\"error\":\"Internal server error: " + e.getMessage() + "\"}");
        }
    }

    private void handleProductsRequest(HttpExchange exchange, String query) throws IOException {
        List<Product> products;
        
        if (query != null && query.startsWith("category=")) {
            String category = query.substring("category=".length());
            products = catalogService.getProductsByCategory(category);
        } else {
            products = catalogService.getAllProducts();
        }

        String jsonResponse = objectMapper.writeValueAsString(products);
        sendResponse(exchange, 200, jsonResponse);
    }

    private void handleProductByIdRequest(HttpExchange exchange, String path) throws IOException {
        String productId = path.substring("/api/products/".length());
        Optional<Product> product = catalogService.getProductById(productId);

        if (product.isPresent()) {
            String jsonResponse = objectMapper.writeValueAsString(product.get());
            sendResponse(exchange, 200, jsonResponse);
        } else {
            sendResponse(exchange, 404, "{\"error\":\"Product not found\"}");
        }
    }

    private void sendResponse(HttpExchange exchange, int statusCode, String response) throws IOException {
        byte[] responseBytes = response.getBytes();
        exchange.sendResponseHeaders(statusCode, responseBytes.length);
        try (OutputStream os = exchange.getResponseBody()) {
            os.write(responseBytes);
        }
    }
}
