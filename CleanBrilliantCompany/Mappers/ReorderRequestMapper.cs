using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Data
{
    public class ReorderRequestMapper : IReorderRequestDB
    {
        private readonly string _connectionString;

        public ReorderRequestMapper(string connectionString)
    {
        _connectionString = connectionString;
    }
        public List<ReorderRequest_RDM> displayListOfReorders()
        {
            var list = new List<ReorderRequest_RDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                SELECT *
                FROM ReorderRequest 
                ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ReorderRequest_RDM
                        {
                            ReorderId = reader.GetInt32(0),
                            ManufacturerId = reader.GetInt32(1),
                            ExpectedDeliveryDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            Status = reader.IsDBNull(3) ? "Pending" : reader.GetString(3),
                        });
                    }

                }
            }

            return list;
        }

        public void createReorderRequest(ReorderRequest_RDM reorder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                // Begin transaction to ensure both main record and products are inserted together
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert the main reorder request record
                        string reorderQuery = @"
                            INSERT INTO dbo.ReorderRequest (ManufacturerId, ExpectedDeliveryDate, Status)
                            OUTPUT INSERTED.ReorderId
                            VALUES (@ManufacturerId, @ExpectedDeliveryDate, @Status);
                        ";
                        
                        int reorderId;
                        
                        using (SqlCommand cmd = new SqlCommand(reorderQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ManufacturerId", reorder.ManufacturerId);
                            cmd.Parameters.AddWithValue("@ExpectedDeliveryDate", reorder.ExpectedDeliveryDate ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Status", reorder.Status ?? "Pending");
                            
                            // Get the newly created ReorderId
                            reorderId = (int)cmd.ExecuteScalar();
                        }
                        
                        // Insert each product for this reorder
                        if (reorder.Products != null && reorder.Products.Count > 0)
                        {
                            string productQuery = @"
                                INSERT INTO dbo.ReorderRequestProduct (ReorderId, ProductId, Quantity, DefectQuantity)
                                VALUES (@ReorderId, @ProductId, @Quantity, @DefectQuantity);
                            ";
                            
                            foreach (var product in reorder.Products)
                            {
                                using (SqlCommand cmd = new SqlCommand(productQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ReorderId", reorderId);
                                    cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                                    cmd.Parameters.AddWithValue("@DefectQuantity", product.DefectQuantity ?? (object)DBNull.Value);
                                    
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        
                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Roll back the transaction if something fails
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }



        public ReorderRequest_RDM getReorderRequestDetails(int reorderId)
        {
            ReorderRequest_RDM reorder = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // First query: Get reorder request details
                string reorderQuery = @"
                    SELECT 
                        reorderId, 
                        manufacturerId, 
                        expectedDeliveryDate, 
                        status
                    FROM 
                        dbo.ReorderRequest 
                    WHERE 
                        reorderId = @ReorderId;
                ";

                using (SqlCommand cmd = new SqlCommand(reorderQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReorderId", reorderId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())  // If a record is found
                        {
                            reorder = new ReorderRequest_RDM
                            {
                                ReorderId = reader.GetInt32(0),
                                ManufacturerId = reader.GetInt32(1),
                                ExpectedDeliveryDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                                Status = reader.IsDBNull(3) ? "Pending" : reader.GetString(3)
                            };
                        }
                    }
                }

                // Second query: Get the products related to the reorder request
                string productQuery = @"
                    SELECT 
                        reorder_product_Id,
                        productId, 
                        quantity, 
                        defectQuantity
                    FROM 
                        dbo.ReorderRequestProduct 
                    WHERE 
                        reorderId = @ReorderId;
                ";

                using (SqlCommand cmd = new SqlCommand(productQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReorderId", reorderId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())  // Read each product for this reorder
                        {
                            var product = new ReorderRequest_Products
                            {
                                ReorderProductId = reader.GetInt32(0),
                                ProductId = reader.GetInt32(1),
                                Quantity = reader.GetInt32(2),
                                DefectQuantity = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                            };
                            reorder.Products.Add(product);  // Add to the list of products
                        }
                    }
                }
            }

            return reorder;  // Return the ReorderRequest with its associated products
        }

        public void updateReorderRequest(ReorderRequest_RDM reorder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Update the ReorderRequest itself (no changes needed here)
                    string reorderQuery = @"
                        UPDATE dbo.ReorderRequest
                        SET ManufacturerId = @ManufacturerId, 
                            ExpectedDeliveryDate = @ExpectedDeliveryDate, 
                            Status = @Status
                        WHERE ReorderId = @ReorderId;
                    ";

                    using (SqlCommand cmd = new SqlCommand(reorderQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ReorderId", reorder.ReorderId);
                        cmd.Parameters.AddWithValue("@ManufacturerId", reorder.ManufacturerId);
                        cmd.Parameters.AddWithValue("@ExpectedDeliveryDate", reorder.ExpectedDeliveryDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", reorder.Status);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            transaction.Rollback();
                            throw new Exception("Reorder request update failed");
                        }
                    }

                    Console.WriteLine("Reorder updated successfully");

                    // Check if there are products to update
                    if (reorder.Products != null && reorder.Products.Count > 0)
                    {
                        Console.WriteLine($"Found {reorder.Products.Count} products to update");
                        
                        // Update each product in the ReorderRequestProduct table
                        foreach (var product in reorder.Products)
                        {
                            if (product.ReorderProductId > 0)
                            {
                                // This is an existing product, update it
                                Console.WriteLine($"Updating product with ReorderProductId: {product.ReorderProductId}");
                                
                                string productQuery = @"
                                    UPDATE dbo.ReorderRequestProduct
                                    SET productId = @ProductId,
                                        Quantity = @Quantity,
                                        DefectQuantity = @DefectQuantity
                                    WHERE reorder_product_Id = @ReorderProductId;  
                                ";

                                using (SqlCommand cmd = new SqlCommand(productQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ReorderProductId", product.ReorderProductId);
                                    cmd.Parameters.AddWithValue("@ProductId", product.ProductId);  
                                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                                    cmd.Parameters.AddWithValue("@DefectQuantity", product.DefectQuantity ?? (object)DBNull.Value);

                                    Console.WriteLine("Executing product update query");
                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    
                                    if (rowsAffected == 0)
                                    {
                                        Console.WriteLine($"No rows updated for Product ID: {product.ProductId}, ReorderProductId: {product.ReorderProductId}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Updated {rowsAffected} row(s) for Product ID: {product.ProductId}, ReorderProductId: {product.ReorderProductId}");
                                    }
                                }
                            }
                            else
                            {
                                // This is a new product, insert it
                                Console.WriteLine($"Inserting new product with Product ID: {product.ProductId}");
                                
                                string insertQuery = @"
                                    INSERT INTO dbo.ReorderRequestProduct (
                                        ReorderId, 
                                        ProductId, 
                                        Quantity, 
                                        DefectQuantity
                                    ) VALUES (
                                        @ReorderId, 
                                        @ProductId, 
                                        @Quantity, 
                                        @DefectQuantity
                                    );
                                ";

                                using (SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ReorderId", reorder.ReorderId);
                                    cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                                    cmd.Parameters.AddWithValue("@DefectQuantity", product.DefectQuantity ?? (object)DBNull.Value);

                                    Console.WriteLine("Executing product insert query");
                                    int rowsAffected = cmd.ExecuteNonQuery();
                                    
                                    if (rowsAffected == 0)
                                    {
                                        Console.WriteLine($"Failed to insert Product ID: {product.ProductId}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Inserted new product with Product ID: {product.ProductId}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products to update");
                    }

                    // Commit the transaction if all updates are successful
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");
                }
                catch (Exception ex)
                {
                    // Rollback if any exception occurs
                    transaction.Rollback();
                    Console.WriteLine($"Transaction rolled back due to error: {ex.Message}");
                    throw;  // Rethrow the exception
                }
            }
        }

        public void cancelReorderRequest(int reorderId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // First delete the associated products from ReorderRequestProduct table
                    string deleteProductsQuery = @"DELETE FROM dbo.ReorderRequestProduct WHERE ReorderId = @ReorderId";
                    using (SqlCommand cmd = new SqlCommand(deleteProductsQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ReorderId", reorderId);
                        cmd.ExecuteNonQuery();
                    }

                    // Then delete the reorder request itself
                    string deleteReorderQuery = @"DELETE FROM dbo.ReorderRequest WHERE ReorderId = @ReorderId";
                    using (SqlCommand cmd = new SqlCommand(deleteReorderQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ReorderId", reorderId);
                        cmd.ExecuteNonQuery();
                    }

                    // Commit the transaction if all deletions are successful
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback if any exception occurs
                    transaction.Rollback();
                    Console.WriteLine($"Transaction rolled back due to error: {ex.Message}");
                    throw;  // Rethrow the exception
                }
            }
        }
        public void deleteProductFromReorder(int reorder_product_Id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Delete the product from the ReorderRequestProduct table
                string query = @"
                    DELETE FROM dbo.ReorderRequestProduct
                    WHERE reorder_product_Id = @ReorderProductId;
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReorderProductId", reorder_product_Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}