﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troy.Model.Branch;

namespace Troy.Data.Repository
{
    public interface IBranchRepository
    {
        List<Branch> GetAllBranch();

        List<Branch> GetFilterBranch(string searchColumn, string searchString, Guid userId);


        Branch FindOneBranchById(int qId);

        //  List<BranchList> GetAddressList();

        List<BranchList> GetAddressList();

        List<CountryList> GetAddressList();

        bool InsertFileUploadDetails(List<Branch> branch);

        bool AddNewBranch(Branch branch);
    }
}
