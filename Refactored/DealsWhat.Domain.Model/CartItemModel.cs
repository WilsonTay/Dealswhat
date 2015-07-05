using System;
using System.Collections.Generic;

namespace DealsWhat.Domain.Model
{
    public class CartItemModel : IEntity
    {
        public DealOptionModel DealOption { get; private set; }

        public IEnumerable<DealAttributeModel> AttributeValues
        {
            get
            {
                return attributeValues;
            }
        }

        private readonly List<DealAttributeModel> attributeValues;

        private CartItemModel()
        {
            attributeValues = new List<DealAttributeModel>();
        }


        public static CartItemModel Create(DealOptionModel dealOption, List<DealAttributeModel> selectedAttributeValues)
        {
            var cartItemModel = new CartItemModel
            {
                DealOption = dealOption
            };

            foreach (var attr in selectedAttributeValues)
            {
                cartItemModel.attributeValues.Add(attr);
            }

            cartItemModel.Key = Guid.NewGuid();

            return cartItemModel;
        }

        public object Key { get; internal set; }
    }
}