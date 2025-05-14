import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../Common/Header';
import Footer from '../Common/Footer';
import accountService from '../../services/accountService';
import transactionService from '../../services/transactionService';
import Notification from '../Common/Notification';

const BillPayments = () => {
  const [accounts, setAccounts] = useState([]);
  const [selectedAccount, setSelectedAccount] = useState(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');
  const [notification, setNotification] = useState(null);
  const navigate = useNavigate();

  // Bill payment form state
  const [formData, setFormData] = useState({
    accountId: '',
    billType: 'electricity',
    provider: '',
    billNumber: '',
    amount: '',
    note: ''
  });

  // Predefined bill types and providers
  const billTypes = [
    { id: 'electricity', name: 'Electricity' },
    { id: 'water', name: 'Water' },
    { id: 'internet', name: 'Internet' },
    { id: 'phone', name: 'Phone' },
    { id: 'gas', name: 'Natural Gas' },
    { id: 'tax', name: 'Taxes' },
    { id: 'other', name: 'Other' }
  ];

  const providers = {
    electricity: ['Enel', 'CEZ', 'E.ON', 'Electrica'],
    water: ['ApaNova', 'Aquatim', 'Raja', 'CAA'],
    internet: ['Orange', 'Telekom', 'RCS-RDS', 'Vodafone'],
    phone: ['Orange', 'Vodafone', 'Telekom', 'DIGI'],
    gas: ['E.ON', 'Engie', 'Distrigaz'],
    tax: ['ANAF', 'Local Taxes', 'Property Tax'],
    other: ['Other Provider']
  };

  // Fetch accounts when component mounts
  useEffect(() => {
    const fetchAccounts = async () => {
      try {
        const data = await accountService.getAccounts();
        setAccounts(data);
        
        // Set first account as default if available
        if (data.length > 0) {
          setSelectedAccount(data[0]);
          setFormData(prev => ({
            ...prev,
            accountId: data[0].id
          }));
        }
      } catch (err) {
        setError('Failed to load accounts');
      } finally {
        setLoading(false);
      }
    };

    fetchAccounts();
  }, []);

  // Update provider when bill type changes
  useEffect(() => {
    if (formData.billType && providers[formData.billType]) {
      setFormData(prev => ({
        ...prev,
        provider: providers[formData.billType][0]
      }));
    }
  }, [formData.billType]);

  // Handle form input changes
  const handleChange = (e) => {
    const { name, value } = e.target;
    
    if (name === 'accountId') {
      const accountId = parseInt(value, 10);
      const account = accounts.find(acc => acc.id === accountId);
      setSelectedAccount(account);
    }
    
    setFormData({
      ...formData,
      [name]: value
    });
  };

  // Validate form before submission
  const validateForm = () => {
    if (!formData.accountId) {
      setError('Please select an account');
      return false;
    }
    
    if (!formData.billType || !formData.provider) {
      setError('Please select bill type and provider');
      return false;
    }
    
    if (!formData.billNumber) {
      setError('Please enter a bill number or client ID');
      return false;
    }
    
    const amount = parseFloat(formData.amount);
    if (isNaN(amount) || amount <= 0) {
      setError('Please enter a valid amount');
      return false;
    }
    
    // Check if user has sufficient balance
    if (selectedAccount && selectedAccount.balance < amount) {
      setError('Insufficient balance for this payment');
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
    
    setSubmitting(true);
    
    try {
      // Prepare transaction data
      const transactionData = {
        fromAccountId: parseInt(formData.accountId, 10),
        amount: parseFloat(formData.amount),
        currency: selectedAccount.currency,
        type: 'BillPayment',
        note: `${formData.billType} bill - ${formData.provider} - ${formData.billNumber}${formData.note ? ` - ${formData.note}` : ''}`,
        toAccountNumber: `BILL-${formData.provider.replace(/\s+/g, '')}-${formData.billNumber}`
      };
      
      await transactionService.createTransaction(transactionData);
      
      // Show success notification
      setNotification({
        type: 'success',
        message: 'Bill payment successful! The payment has been processed.'
      });
      
      // Reset form
      setFormData({
        ...formData,
        billNumber: '',
        amount: '',
        note: ''
      });
    } catch (err) {
      setError(err.message || 'Payment failed. Please try again.');
    } finally {
      setSubmitting(false);
    }
  };

  // Handle cancel button
  const handleCancel = () => {
    navigate('/dashboard');
  };

  if (loading) {
    return <div className="text-center p-8">Loading...</div>;
  }

  return (
    <div className="min-h-screen bg-gray-100 flex flex-col">
      <Header />
      
      <main className="flex-grow max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <div className="px-4 py-6 sm:px-0">
          <h1 className="text-2xl font-semibold text-gray-900 mb-6">
            Pay Your Bills
          </h1>
          
          {notification && (
            <div className="mb-6">
              <Notification
                type={notification.type}
                message={notification.message}
                onClose={() => setNotification(null)}
              />
            </div>
          )}
          
          {accounts.length === 0 ? (
            <div className="bg-white shadow-lg rounded-lg p-6 text-center">
              <p className="text-gray-700 mb-4">You don't have any accounts available for bill payments.</p>
              <button
                onClick={() => navigate('/dashboard')}
                className="btn-primary"
              >
                Go to Dashboard
              </button>
            </div>
          ) : (
            <div className="bg-white shadow-lg rounded-lg">
              <div className="px-4 py-5 sm:p-6">
                <h2 className="text-lg font-medium text-gray-900 mb-4">
                  Bill Payment Form
                </h2>
                
                {error && (
                  <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                    {error}
                  </div>
                )}
                
                <form onSubmit={handleSubmit} className="space-y-6">
                  {/* Source account selection */}
                  <div>
                    <label htmlFor="accountId" className="block text-sm font-medium text-gray-700 mb-1">
                      From Account
                    </label>
                    <select
                      id="accountId"
                      name="accountId"
                      value={formData.accountId}
                      onChange={handleChange}
                      className="input-field"
                      required
                    >
                      <option value="">Select an account</option>
                      {accounts.map(account => (
                        <option key={account.id} value={account.id}>
                          {account.accountNumber} ({account.currency} {account.balance.toFixed(2)})
                        </option>
                      ))}
                    </select>
                  </div>
                  
                  {/* Bill type and provider */}
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                      <label htmlFor="billType" className="block text-sm font-medium text-gray-700 mb-1">
                        Bill Type
                      </label>
                      <select
                        id="billType"
                        name="billType"
                        value={formData.billType}
                        onChange={handleChange}
                        className="input-field"
                        required
                      >
                        {billTypes.map(type => (
                          <option key={type.id} value={type.id}>
                            {type.name}
                          </option>
                        ))}
                      </select>
                    </div>
                    
                    <div>
                      <label htmlFor="provider" className="block text-sm font-medium text-gray-700 mb-1">
                        Provider
                      </label>
                      <select
                        id="provider"
                        name="provider"
                        value={formData.provider}
                        onChange={handleChange}
                        className="input-field"
                        required
                      >
                        {(providers[formData.billType] || []).map(provider => (
                          <option key={provider} value={provider}>
                            {provider}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                  
                  {/* Bill information */}
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                      <label htmlFor="billNumber" className="block text-sm font-medium text-gray-700 mb-1">
                        Bill Number / Client ID
                      </label>
                      <input
                        id="billNumber"
                        name="billNumber"
                        type="text"
                        value={formData.billNumber}
                        onChange={handleChange}
                        className="input-field"
                        placeholder="Enter bill number or client ID"
                        required
                      />
                    </div>
                    
                    <div>
                      <label htmlFor="amount" className="block text-sm font-medium text-gray-700 mb-1">
                        Amount {selectedAccount ? `(${selectedAccount.currency})` : ''}
                      </label>
                      <input
                        id="amount"
                        name="amount"
                        type="number"
                        value={formData.amount}
                        onChange={handleChange}
                        className="input-field"
                        placeholder="Enter payment amount"
                        min="0.01"
                        step="0.01"
                        required
                      />
                    </div>
                  </div>
                  
                  {/* Optional note */}
                  <div>
                    <label htmlFor="note" className="block text-sm font-medium text-gray-700 mb-1">
                      Note (Optional)
                    </label>
                    <input
                      id="note"
                      name="note"
                      type="text"
                      value={formData.note}
                      onChange={handleChange}
                      className="input-field"
                      placeholder="Add a note for this payment"
                    />
                  </div>
                  
                  {/* Action buttons */}
                  <div className="flex justify-end space-x-4 pt-4">
                    <button
                      type="button"
                      onClick={handleCancel}
                      className="btn-secondary"
                    >
                      Cancel
                    </button>
                    <button
                      type="submit"
                      disabled={submitting}
                      className="btn-primary"
                    >
                      {submitting ? 'Processing...' : 'Pay Bill'}
                    </button>
                  </div>
                </form>
              </div>
            </div>
          )}
        </div>
      </main>
      
      <Footer />
    </div>
  );
};

export default BillPayments;