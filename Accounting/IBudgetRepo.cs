using System;
using System.Collections.Generic;
using System.Text;

namespace Accounting
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}