// Modal component for transferring money between accounts
import React, { useState } from 'react';
import transactionService from '../../services/transactionService';

// This component handles money transfers with form validation and error handling
const TransferMoney = ({ accounts, onClose, onSuccess }) => {
  // Form state management - keeping track of all transfer details
  const [transferData, setTransferData] = useState({
    fromAccountId: accounts[0]?.id || '', // Default to first account if available
    toAccountNumber: '',                   // Account number to transfer to
    amount: '',                            // Transfer amount
    currency: accounts[0]?.currency || '', // Currency of the transfer
    note: ''                              // Optional note for the transfer
  });
  
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  // Handle form field changes
  const handleChange = (e) => {
    const { name, value } = e.target;
    setTransferData({
      ...transferData,
      [name]: value
    });
  };

  // Update currency when source account changes
  const handleAccountChange = (e) => {
    const accountId = parseInt(e.target.value);
    const selectedAccount = accounts.find(a => a.id === accountId);
    
    setTransferData({
      ...transferData,
      fromAccountId: accountId,
      currency: selectedAccount?.currency || ''
    });
  };

  // Form validation before submission
  const validateForm = () => {
    // Check if all required fields are filled
    if (!transferData.fromAccountId || !transferData.toAccountNumber || !transferData.amount) {
      setError('Please fill in all required fields');
      return false;
    }

    // Validate amount is a positive number
    const amount = parseFloat(transferData.amount);
    if (isNaN(amount) || amount <= 0) {
      setError('Please enter a valid amount');
      return false;
    }

    // Check if user has sufficient balance
    const sourceAccount = accounts.find(a => a.id === parseInt(transferData.fromAccountId));
    if (sourceAccount && sourceAccount.balance < amount) {
      setError('Insufficient balance');
      return false;
    }

    return true;
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (!validateForm()) {
      return;
    }

    setLoading(true);

    try {
      // Create transaction data object
      const transactionData = {
        fromAccountId: parseInt(transferData.fromAccountId),
        amount: parseFloat(transferData.amount),
        currency: transferData.currency,
        type: 'Transfer',
        note: transferData.note,
        // For simplicity, we're using account number directly. 
        // In a real app, we'd convert this to toAccountId
        toAccountNumber: transferData.toAccountNumber
      };

      await transactionService.createTransaction(transactionData);
      onSuccess(); // Notify parent component of successful transfer
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    // Modal overlay
    <div className="fixed inset-0 bg-gray-600 bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-8 max-w-md w-full">
        <h2 className="text-2xl font-bold mb-6">Transfer Money</h2>
        
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Source account selection */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              From Account
            </label>
            <select
              name="fromAccountId"
              value={transferData.fromAccountId}
              onChange={handleAccountChange}
              className="input-field"
              required
            >
              {accounts.map(account => (
                <option key={account.id} value={account.id}>
                  {account.accountNumber} ({account.currency} {account.balance.toFixed(2)})
                </option>
              ))}
            </select>
          </div>

          {/* Destination account number */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              To Account Number
            </label>
            <input
              type="text"
              name="toAccountNumber"
              value={transferData.toAccountNumber}
              onChange={handleChange}
              className="input-field"
              placeholder="Enter account number"
              required
            />
          </div>

          {/* Transfer amount */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Amount ({transferData.currency})
            </label>
            <input
              type="number"
              name="amount"
              value={transferData.amount}
              onChange={handleChange}
              className="input-field"
              placeholder="Enter amount"
              min="0.01"
              step="0.01"
              required
            />
          </div>

          {/* Optional note */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Note (Optional)
            </label>
            <input
              type="text"
              name="note"
              value={transferData.note}
              onChange={handleChange}
              className="input-field"
              placeholder="Add a note"
            />
          </div>

          {/* Action buttons */}
          <div className="flex justify-end space-x-4 mt-6">
            <button
              type="button"
              onClick={onClose}
              className="btn-secondary"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="btn-primary"
            >
              {loading ? 'Processing...' : 'Transfer'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default TransferMoney;