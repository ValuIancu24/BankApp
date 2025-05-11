import React, { useState, useEffect } from 'react';
import transactionService from '../../services/transactionService';

const TransactionHistory = ({ accountId }) => {
  const [transactions, setTransactions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [filter, setFilter] = useState('all'); // all, important, incoming, outgoing
  const [sortOrder, setSortOrder] = useState('desc'); // asc, desc

  useEffect(() => {
    if (accountId) {
      fetchTransactions();
    }
  }, [accountId]);

  const fetchTransactions = async () => {
    try {
      setLoading(true);
      const data = await transactionService.getAccountTransactions(accountId);
      setTransactions(data);
      setError('');
    } catch (err) {
      setError('Failed to load transactions');
      console.error('Transaction fetch error:', err);
    } finally {
      setLoading(false);
    }
  };

  // Filter transactions based on selected filter
  const filteredTransactions = transactions.filter(transaction => {
    switch (filter) {
      case 'important':
        return transaction.isImportant;
      case 'incoming':
        // A transaction is incoming if this account is the recipient
        return (
          transaction.type === 'Deposit' || 
          (transaction.toAccountId && transaction.toAccountId === accountId)
        );
      case 'outgoing':
        // A transaction is outgoing if this account is the sender
        return (
          transaction.type === 'Withdrawal' || 
          transaction.type === 'Transfer' || 
          transaction.type === 'BillPayment' ||
          (transaction.fromAccountId && transaction.fromAccountId === accountId)
        );
      default:
        return true;
    }
  });

  // Sort transactions by date
  const sortedTransactions = [...filteredTransactions].sort((a, b) => {
    const dateA = new Date(a.createdAt).getTime();
    const dateB = new Date(b.createdAt).getTime();
    return sortOrder === 'desc' ? dateB - dateA : dateA - dateB;
  });

  // Mark transaction as important
  const handleMarkImportant = async (transactionId) => {
    try {
      await transactionService.markTransactionAsImportant(transactionId);
      fetchTransactions();
    } catch (err) {
      console.error('Failed to mark transaction as important:', err);
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
    // Deposits and incoming transfers are considered incoming
    const isIncoming = transaction.type === 'Deposit' || 
                       (transaction.toAccountId && transaction.toAccountId === accountId);
    
    return {
      amountColor: isIncoming ? 'text-green-600' : 'text-red-600',
      amountPrefix: isIncoming ? '+' : '-',
      icon: isIncoming ? '↓' : '↑'
    };
  };

  // Toggle sort order
  const toggleSortOrder = () => {
    setSortOrder(sortOrder === 'desc' ? 'asc' : 'desc');
  };

  if (loading && transactions.length === 0) {
    return <div className="text-center p-4">Loading transactions...</div>;
  }

  return (
    <div className="bg-white overflow-hidden shadow-lg rounded-lg">
      <div className="px-4 py-5 sm:p-6">
        <div className="flex justify-between items-center mb-4">
          <h3 className="text-lg font-medium text-gray-900">
            Transaction History
          </h3>
          
          <div className="flex space-x-2">
            {/* Filter dropdown */}
            <select
              value={filter}
              onChange={(e) => setFilter(e.target.value)}
              className="input-field"
            >
              <option value="all">All Transactions</option>
              <option value="important">Important Only</option>
              <option value="incoming">Incoming</option>
              <option value="outgoing">Outgoing</option>
            </select>
            
            {/* Sort order button */}
            <button
              onClick={toggleSortOrder}
              className="px-2 py-1 bg-gray-200 rounded hover:bg-gray-300"
              title={sortOrder === 'desc' ? 'Newest first' : 'Oldest first'}
            >
              {sortOrder === 'desc' ? '↓' : '↑'}
            </button>
          </div>
        </div>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        {sortedTransactions.length === 0 ? (
          <p className="text-gray-500 text-center py-4">No transactions found</p>
        ) : (
          <div className="space-y-4">
            {sortedTransactions.map(transaction => {
              const style = getTransactionStyle(transaction);
              
              return (
                <div 
                  key={transaction.id}
                  className={`p-4 rounded-lg border ${
                    transaction.isImportant ? 'border-yellow-400 bg-yellow-50' : 'border-gray-200'
                  }`}
                >
                  <div className="flex justify-between items-start">
                    <div>
                      <div className="flex items-center">
                        <span className="mr-2">{style.icon}</span>
                        <p className="font-medium capitalize">{transaction.type}</p>
                      </div>
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
                    
                    {transaction.type === 'Transfer' && (
                      <p className="text-sm text-gray-500">
                        {transaction.fromAccountId === accountId 
                          ? `To: ${transaction.toAccountNumber}`
                          : `From: ${transaction.fromAccountNumber}`}
                      </p>
                    )}
                    
                    {transaction.type === 'BillPayment' && (
                      <p className="text-sm text-gray-500">
                        Bill payment
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