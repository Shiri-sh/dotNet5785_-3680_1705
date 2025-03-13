using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class BlImplementation: IBl
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public ICall Call => throw new NotImplementedException();

    public IVolunteer Volunteer => throw new NotImplementedException();

    public IAdmin Admin => throw new NotImplementedException();
}
