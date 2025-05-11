// Transaction history component with filtering and sorting capabilities
import React, { useState, useEffect } from 'react';
import transactionService from '../../services/transactionService';

const TransactionHistory = ({ accountId }) => {
  const [transactions, setTransactions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [filter, setFilter] = useState('all'); // all, important, incoming, outgoing

  useEffect(() => {
    fetchTransactions();
  }, [accountId]);

  const fetchTransactions = async () => {
    try {
      const data = await transactionService.getAccountTransactions(accountId);
      setTransactions(data);
    } catch (err) {
      setError('Failed to load transactions');
    } finally {
      setLoading(false);
    }
  };

  // Filter transactions based on selected filter
  const filteredTransactions = transactions.filter(t => {
    switch (filter) {
      case 'important':
        return t.isImportant;
      case 'incoming':
        return t.toAccountNumber === transactions.find(tx => tx.fromAccountId === accountId)?.fromAccountNumber;
      case 'outgoing':
        return t.fromAccountNumber === transactions.find(tx => tx.fromAccountId === accountId)?.fromAccountNumber;
      default:
        return true;
    }
  });

  // Mark transaction as important
  const handleMarkImportant = async (transactionId) => {
    try {
      await transactionService.markTransactionAsImportant(transactionId);
      fetchTransactions();
    } catch (err) {
      console.error('Failed to mark transaction as important');
    }
  };

  // Format date for display
  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  // Determine transaction direction and styling
  const getTransactionStyle = (transaction) => {
    const isIncoming = transaction.toAccountNumber === 
      transactions.find(t => t.fromAccountId === accountId)?.fromAccountNumber;
    
    return {
      amountColor: isIncoming ? 'text-green-600' : 'text-red-600',
      amountPrefix: isIncoming ? '+' : '-'
    };
  };

  if (loading) return <div className="text-center p-4">Loading transactions...</div>;

  return (
    <div className="bg-white overflow-hidden shadow-lg rounded-lg">
      <div className="px-4 py-5 sm:p-6">
        <div className="flex justify-between items-center mb-4">
          <h3 className="text-lg font-medium text-gray-900">
            Transaction History
          </h3>
          
          {/* Filter dropdown */}
          <select
            value={filter}
            onChange={(e) => setFilter(e.target.value)}
            className="input-field w-40"
          >
            <option value="all">All Transactions</option>
            <option value="important">Important Only</option>
            <option value="incoming">Incoming</option>
            <option value="outgoing">Outgoing</option>
          </select>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        {filteredTransactions.length === 0 ? (
          <p className="text-gray-500 text-center py-4">No transactions found</p>
        ) : (
          <div className="space-y-4">
            {filteredTransactions.map(transaction => {
              const style = getTransactionStyle(transaction);
              
              return (
                <div 
                  key={transaction.id}
                  className={`p-4 rounded-lg border ${
                    transaction.isImportant ? 'border-yellow-400' : 'border-gray-200'
                  }`}
                >
                  <div className="flex justify-between items-start">
                    <div>
                      <p className="font-medium capitalize">{transaction.type}</p>
                      <p className="text-sm text-gray-500">{formatDate(transaction.createdAt)}</p>
                      {transaction.note && (
                        <p className="text-sm text-gray-600 mt-1">Note: {transaction.note}</p>
                      )}
                    </div>
                    
                    <div className="text-right">
                      <p className={`font-bold ${style.amountColor}`}>
                        {style.amountPrefix}{transaction.currency} {transaction.amount.toFixed(2)}
                      </p>
                      <p className="text-sm text-gray-500">{transaction.status}</p>
                    </div>
                  </div>
                  
                  <div className="mt-4 flex justify-between items-center">
                    <div className="flex space-x-2">
                      <button
                        onClick={() => handleMarkImportant(transaction.id)}
                        className={`text-sm ${
                          transaction.isImportant ? 'text-yellow-600' : 'text-gray-600'
                        } hover:text-yellow-500`}
                      >
                        {transaction.isImportant ? '★ Important' : '☆ Mark as Important'}
                      </button>
                    </div>
                    
                    {transaction.toAccountNumber && (
                      <p className="text-sm text-gray-500">
                        To: {transaction.toAccountNumber}
                      </p>
                    )}
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

export default TransactionHistory;