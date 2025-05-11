// Main dashboard component that orchestrates account display and transactions
import React, { useState, useEffect } from 'react';
import accountService from '../../services/accountService';
import AccountOverview from './AccountOverview';
import TransactionHistory from './TransactionHistory';
import TransferMoney from './TransferMoney';
import Header from '../Common/Header';

const Dashboard = () => {
  // State management for accounts and loading/error states
  const [accounts, setAccounts] = useState([]);
  const [selectedAccount, setSelectedAccount] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showTransferModal, setShowTransferModal] = useState(false);

  // Fetch user accounts when component mounts
  useEffect(() => {
    fetchAccounts();
  }, []);

  const fetchAccounts = async () => {
    try {
      const data = await accountService.getAccounts();
      setAccounts(data);
      // Select first account by default if available
      if (data.length > 0) {
        setSelectedAccount(data[0]);
      }
    } catch (err) {
      setError('Failed to load accounts');
    } finally {
      setLoading(false);
    }
  };

  // Handle successful transactions by refreshing account data
  const handleTransactionSuccess = () => {
    fetchAccounts();
    setShowTransferModal(false);
  };

  if (loading) return <div className="text-center p-4">Loading...</div>;
  if (error) return <div className="text-red-500 text-center p-4">{error}</div>;

  return (
    <div className="min-h-screen bg-gray-100">
      <Header />
      
      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        {/* Welcome section with primary actions */}
        <div className="px-4 py-6 sm:px-0">
          <h1 className="text-2xl font-semibold text-gray-900">
            Welcome to Your BankApp Dashboard
          </h1>
          
          <div className="mt-4">
            <button
              onClick={() => setShowTransferModal(true)}
              className="btn-primary"
            >
              Transfer Money
            </button>
          </div>

          {/* Main content area with grid layout */}
          <div className="mt-8 grid grid-cols-1 gap-6 lg:grid-cols-2">
            {/* Account Overview section */}
            <div className="bg-white overflow-hidden shadow-lg rounded-lg">
              <div className="px-4 py-5 sm:p-6">
                <h2 className="text-lg font-medium text-gray-900 mb-4">
                  Your Accounts
                </h2>
                
                {accounts.length === 0 ? (
                  <p className="text-gray-500">No accounts found</p>
                ) : (
                  <div className="space-y-4">
                    {accounts.map(account => (
                      <div 
                        key={account.id}
                        className={`p-4 rounded-lg border-2 cursor-pointer transition-colors
                          ${selectedAccount?.id === account.id 
                            ? 'border-blue-500 bg-blue-50' 
                            : 'border-gray-200 hover:border-gray-300'}`}
                        onClick={() => setSelectedAccount(account)}
                      >
                        <div className="flex justify-between items-center">
                          <div>
                            <p className="font-medium">{account.currency} Account</p>
                            <p className="text-sm text-gray-500">{account.accountNumber}</p>
                          </div>
                          <div className="text-right">
                            <p className="font-bold">
                              {account.currency} {account.balance.toFixed(2)}
                            </p>
                            <p className="text-sm text-gray-500">{account.status}</p>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </div>

            {/* Selected account details and transaction history */}
            <div className="space-y-6">
              {selectedAccount && (
                <>
                  <AccountOverview 
                    account={selectedAccount} 
                    onUpdate={fetchAccounts}
                  />
                  <TransactionHistory 
                    accountId={selectedAccount.id} 
                  />
                </>
              )}
            </div>
          </div>
        </div>
      </main>

      {/* Transfer money modal */}
      {showTransferModal && (
        <TransferMoney
          accounts={accounts}
          onClose={() => setShowTransferModal(false)}
          onSuccess={handleTransactionSuccess}
        />
      )}
    </div>
  );
};

export default Dashboard;