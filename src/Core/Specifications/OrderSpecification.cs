using Core.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrderSpecification: BaseSpecification<Order>
{
    public OrderSpecification(string email): base(o => o.BuyerEmail == email)
    {
        AddInclude(o => o.DeliveryMethod);
        AddInclude(o => o.OrderItems);
        AddOrderByDescending(o => o.OrderDate);
    }

    public OrderSpecification(int id, string email): base(o => o.Id == id && o.BuyerEmail == email)
    {
        AddInclude("OrderItems");
        AddInclude("DeliveryMethod");
    }
}       

