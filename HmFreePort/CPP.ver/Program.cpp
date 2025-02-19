﻿#include <iostream>
#include <vector>
#include <winsock2.h>
#include <iphlpapi.h>

#pragma comment(lib, "iphlpapi.lib")
#pragma comment(lib, "ws2_32.lib")

std::vector<int> GetUsedPorts() {
    std::vector<int> usedPorts;
    ULONG outBufLen = sizeof(MIB_TCPTABLE2);
    PMIB_TCPTABLE2 pTcpTable = (MIB_TCPTABLE2*)malloc(outBufLen);

    if (GetExtendedTcpTable(pTcpTable, &outBufLen, FALSE, AF_INET, TCP_TABLE_OWNER_PID_ALL, 0) == NO_ERROR) {
        for (unsigned int i = 0; i < pTcpTable->dwNumEntries; ++i) {
            usedPorts.push_back(ntohs(pTcpTable->table[i].dwLocalPort));
        }
    }

    free(pTcpTable);

    outBufLen = sizeof(MIB_UDPTABLE);
    PMIB_UDPTABLE pUdpTable = (MIB_UDPTABLE*)malloc(outBufLen);

    if (GetExtendedUdpTable(pUdpTable, &outBufLen, FALSE, AF_INET, UDP_TABLE_OWNER_PID, 0) == NO_ERROR) {
        for (unsigned int i = 0; i < pUdpTable->dwNumEntries; ++i) {
            usedPorts.push_back(ntohs(pUdpTable->table[i].dwLocalPort));
        }
    }

    free(pUdpTable);

    return usedPorts;
}

int getFreePort() {
    std::vector<int> portsInUse = GetUsedPorts();

    for (int port = 49152; port <= 65535; ++port) {
        if (std::find(portsInUse.begin(), portsInUse.end(), port) == portsInUse.end()) {
            return port;
        }
    }

    return 0; // 空きポートが見つからない場合
}

int main() {
    int port = getFreePort();
    std::cout << port << std::endl;
    return (port > 0) ? 0 : 1;
}
