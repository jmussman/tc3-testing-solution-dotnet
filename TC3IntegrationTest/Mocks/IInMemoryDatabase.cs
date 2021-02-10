// IInMemoryDatabase.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System.Data.Common;

namespace TC3IntegrationTest.Mocks {

    public interface IInMemoryDatabase {

        DbConnection Connection { get; }
    }
}
