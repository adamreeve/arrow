// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Threading.Tasks;

namespace Apache.Arrow.Flight.IntegrationTest;

public class FlightClientCommand
{
    private readonly int _port;
    private readonly string _scenario;
    private readonly FileInfo _jsonFileInfo;

    public FlightClientCommand(int port, string scenario, FileInfo jsonFileInfo)
    {
        _port = port;
        _scenario = scenario;
        _jsonFileInfo = jsonFileInfo;
    }

    public async Task Execute()
    {
        if (!string.IsNullOrEmpty(_scenario))
        {
            // No named scenarios are currently implemented
            throw new Exception($"Scenario '{_scenario}' is not supported.");
        }

        if (!(_jsonFileInfo?.Exists ?? false))
        {
            throw new Exception($"Invalid JSON file path '{_jsonFileInfo?.FullName}'");
        }

        var scenario = new JsonTestScenario(_port, _jsonFileInfo);
        await scenario.RunClient().ConfigureAwait(false);
    }
}
