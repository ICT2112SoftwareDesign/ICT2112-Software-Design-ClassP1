using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Control
{
    public class TransferControl
    {
        private readonly IWarehouse _iWarehouseInterface ;
        private readonly IItemUpdate _iItemUpdateInterface;

        //private readonly IItemUpdate _iItemUpdateInterface;

        private readonly TransferMapper _transferMapper;

        public TransferControl(string connectionString, IWarehouse iWarehouseInterface, IItemUpdate iItemUpdateInterface)
        {
            _transferMapper = new TransferMapper(connectionString);
            _iWarehouseInterface = iWarehouseInterface;
            _iItemUpdateInterface = iItemUpdateInterface;
           // _iItemUpdateInterface = iItemUpdateInterface;
            Console.WriteLine("Warehouses loaded from database.");
        }

        public async Task<Warehouse> getWarehouseDetails(int warehouseId)
        {
            Warehouse warehouse = await _iWarehouseInterface.getWarehouseDetails(warehouseId);
            return await Task.FromResult(warehouse);
            //return await _iWarehouseInterface.getWarehouseDetails(warehouseId);
        }

        public async Task<List<Item>> getItemByProductAndWarehouse(int productId, int quantity, int warehouseId)
        {
            return await _iWarehouseInterface.getItemByProductAndWarehouse(productId, quantity, warehouseId);
        }

        public async Task<int> getProductQuantityByWarehouse(int productId, int warehouseId)
        {
            return await _iWarehouseInterface.getProductQuantityByWarehouse(productId, warehouseId);
        }

        public async Task<List<Warehouse>> getAllWarehouseDetails()
        {
            return await _iWarehouseInterface.getAllWarehouseDetails();
        }

        public async Task<int> createTransfer(int transferId, int productId, int sourceWarehouseId, int destinationWarehouseId, int quantity, TransferStatus status)
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

        public async Task<bool> updateItemStatus(int itemId, int? reservationId, int? orderId, int? transferId, int? returnId, ItemStatus status){
            return await _iItemUpdateInterface.updateItemStatus(itemId, reservationId, orderId, transferId, returnId, status);
        }

        public async Task<List<Item>> getTransferredItems(int transferId)
        {
            return await _iWarehouseInterface.getTransferredItems(transferId);
        }

        public async Task<bool> updateWarehouseCapacity(int warehouseId)
        {
            return await Task.FromResult(_transferMapper.updateWarehouseCapacity(warehouseId));
        }
    }
}