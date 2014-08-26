﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.ProductGroup;

namespace Troy.Data.Repository
{
    public interface IProductGroupRepository
    {
        List<ProductGroup> GetAllProductGroup();

        List<ProductGroup> GetFilterProductGroup(string searchColumn, string searchString, Guid userId);


        ProductGroup FindOneProductGroupById(int qId);

        //  List<BranchList> GetAddressList();

        bool InsertFileUploadDetails(List<ProductGroup> ProductGroup);

        bool AddNewProductGroup(ProductGroup ProductGroup);

        //bool EditExistingProductGroup(ProductGroup ProductGroup);
    }
}
