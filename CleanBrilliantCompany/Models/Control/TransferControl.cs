using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Control
{
    public class TransferControl : IWarehouse
    {
        private readonly IWarehouse _iWarehouseInterface ;
        private readonly TransferMapper _transferMapper;

        public TransferControl(string connectionString, IWarehouse iWarehouseInterface)
        {
            _transferMapper = new TransferMapper(connectionString);
            _iWarehouseInterface = iWarehouseInterface;
            Console.WriteLine("Warehouses loaded from database.");
        }

        public async Task<Warehouse> getWarehouseDetails(int warehouseId)
        {
            Warehouse warehouse = await _iWarehouseInterface.getWarehouseDetails(warehouseId);
            return await Task.FromResult(warehouse);
            //return await _iWarehouseInterface.getWarehouseDetails(warehouseId);
        }

        public async Task<List<Item>> getItemByProductAndWarehouse(int productId, int warehouseId)
        {
            return await _iWarehouseInterface.getItemByProductAndWarehouse(productId, warehouseId);
        }

        public async Task<int> getProductQuantityByWarehouse(int productId, int warehouseId)
        {
            return await _iWarehouseInterface.getProductQuantityByWarehouse(productId, warehouseId);
        }

        public async Task<List<Warehouse>> getAllWarehouseDetails()
        {
            return await _iWarehouseInterface.getAllWarehouseDetails();
        }

        public async Task<bool> createTransfer(int transferId, int productId, int sourceWarehouseId, int destinationWarehouseId, int quantity, TransferStatus status)
        {
            return await Task.FromResult(_transferMapper.createTransfer(transferId, productId, sourceWarehouseId, destinationWarehouseId, quantity, status));
        }

        public async Task<List<Transfer>> getAllTransfers()
        {
            return await Task.FromResult(_transferMapper.getAllTransfers());
        }

        public async Task<List<Product>> getLowStockProductInWarehouse()
        {
            return await Task.FromResult(_transferMapper.getLowStockProductInWarehouse());
        }

        public async Task<bool> deleteTransfer(int transferId)
        {
            return await Task.FromResult(_transferMapper.deleteTransfer(transferId));
        }

        public async Task<bool> updateTransfer(int transferId, int destinationWarehouse, TransferStatus status)
        {
            return await Task.FromResult(_transferMapper.updateTransfer(transferId, destinationWarehouse, status));
        }
    }
}