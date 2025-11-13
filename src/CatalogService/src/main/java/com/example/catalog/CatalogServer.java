package com.example.catalog;

import com.example.catalog.handler.CatalogHandler;
import com.sun.net.httpserver.HttpServer;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.util.concurrent.Executors;

public class CatalogServer {
    private static final int PORT = 8080;
    private HttpServer server;

    public void start() throws IOException {
        server = HttpServer.create(new InetSocketAddress(PORT), 0);
        
        // Register handlers
        server.createContext("/api/products", new CatalogHandler());
        server.createContext("/health", new CatalogHandler());
        
        // Set thread pool executor
        server.setExecutor(Executors.newFixedThreadPool(10));
        
        // Start the server
        server.start();
        System.out.println("Catalog Service started on port " + PORT);
        System.out.println("Available endpoints:");
        System.out.println("  GET /api/products - Get all products");
        System.out.println("  GET /api/products?category=Electronics - Get products by category");
        System.out.println("  GET /api/products/{id} - Get product by ID");
        System.out.println("  GET /health - Health check");
    }

    public void stop() {
        if (server != null) {
            server.stop(0);
            System.out.println("Catalog Service stopped");
        }
    }

    public static void main(String[] args) {
        CatalogServer catalogServer = new CatalogServer();
        
        try {
            catalogServer.start();
            
            // Add shutdown hook
            Runtime.getRuntime().addShutdownHook(new Thread(catalogServer::stop));
            
            // Keep the server running
            Thread.currentThread().join();
        } catch (Exception e) {
            System.err.println("Error starting catalog service: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
