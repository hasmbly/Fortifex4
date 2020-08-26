window.MetaMask =
{
    ethereum: null,

    IsEthereumWalletInstalled: function ()
    {
        if (typeof window.ethereum !== 'undefined')
        {
            console.log('JS - Ethereum Object: ' + JSON.stringify(window.ethereum));

            MetaMask.ethereum = window.ethereum;

            return true;
        }

        return false;
    },

    // return List<string>
    ConnectToMetaMask: async function ()
    {
        async function getAccount()
        {
            const accounts = await MetaMask.ethereum.request({ method: 'eth_requestAccounts' });

            console.log('JS - All MetaMask Accounts: ' + JSON.stringify(accounts));

            return accounts;
        }

        return getAccount();
    },

    // register this function on MainLayout.razor, notify with modal pop up if MetaMask's account was changed
    OnAccountsChanged: function ()
    {
        MetaMask.ethereum.on('accountsChanged', function (accounts)
        {
            let msg = "JS - Your MetaMask's Account has Changed to " + accounts[0];

            console.log(msg);

            // send "msg" to .NET Object Function
        });
    },

    // for differentiate MetaMask from other ethereum-compatible browsers,
    IsMetaMask: function ()
    {
        if (MetaMask.ethereum.isMetaMask)
        {
            return true;
        }

        return false;
    }
};