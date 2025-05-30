namespace ConsoleApp1.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int WarehouseStock { get; private set; }
        public int DynamicStock { get; private set; }
        
        public Product SetStock(int warehouseStock)
        {
            //one is a stock connected to the warehouse and the other
            //is a dynamic structure connected to the basket. 
            WarehouseStock = warehouseStock;
            DynamicStock = warehouseStock;
            return this;
        }

        public void DecreaseDynamicStock()
        {
            DynamicStock--;
        }

        public bool HasDynamicStock()
        {
            return WarehouseStock - DynamicStock < 0;
        }

        public void IncreaseDynamicStock(int quantity = 1)
        {
            DynamicStock += quantity;
            if (DynamicStock > WarehouseStock)
            {
                DynamicStock = WarehouseStock;
            }
        }

        public void SetWarehouseStock(int updatedWarehouseStock)
        {
            throw new System.NotImplementedException();
        }
    }
}