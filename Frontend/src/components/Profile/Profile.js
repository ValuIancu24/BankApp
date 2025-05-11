// User profile management component
import React, { useState, useEffect } from 'react';
import ChangePassword from './ChangePassword';
import Header from '../Common/Header';

// This component handles user profile display and management
const Profile = () => {
  const [user, setUser] = useState(null);
  const [showChangePassword, setShowChangePassword] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    // In a real app, we'd fetch user data from the API
    // For now, we'll use data from localStorage
    const fetchUserData = () => {
      try {
        const userData = JSON.parse(localStorage.getItem('userData') || '{}');
        setUser(userData);
      } catch (err) {
        setError('Failed to load user data');
      } finally {
        setLoading(false);
      }
    };

    fetchUserData();
  }, []);

  if (loading) return <div className="text-center p-4">Loading...</div>;
  if (error) return <div className="text-red-500 text-center p-4">{error}</div>;

  return (
    <div className="min-h-screen bg-gray-100">
      <Header />
      
      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <div className="px-4 py-6 sm:px-0">
          <h1 className="text-2xl font-semibold text-gray-900 mb-6">
            Profile Settings
          </h1>

          <div className="space-y-6">
            {/* Profile information card */}
            <div className="bg-white shadow-lg rounded-lg">
              <div className="px-4 py-5 sm:p-6">
                <h3 className="text-lg font-medium text-gray-900 mb-6">
                  Personal Information
                </h3>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div>
                    <label className="text-sm font-medium text-gray-500">Full Name</label>
                    <p className="mt-1 text-lg text-gray-900">
                      {user?.firstName || ''} {user?.lastName || ''}
                    </p>
                  </div>
                  
                  <div>
                    <label className="text-sm font-medium text-gray-500">Email</label>
                    <p className="mt-1 text-lg text-gray-900">{user?.email || ''}</p>
                  </div>
                  
                  <div>
                    <label className="text-sm font-medium text-gray-500">Username</label>
                    <p className="mt-1 text-lg text-gray-900">{user?.username || ''}</p>
                  </div>
                  
                  <div>
                    <label className="text-sm font-medium text-gray-500">Phone Number</label>
                    <p className="mt-1 text-lg text-gray-900">{user?.phoneNumber || ''}</p>
                  </div>
                  
                  <div>
                    <label className="text-sm font-medium text-gray-500">City</label>
                    <p className="mt-1 text-lg text-gray-900">{user?.city || ''}</p>
                  </div>
                  
                  <div>
                    <label className="text-sm font-medium text-gray-500">Account Status</label>
                    <p className="mt-1 text-lg text-gray-900">
                      {user?.isActive ? 'Active' : 'Inactive'}
                    </p>
                  </div>
                </div>
              </div>
            </div>

            {/* Account actions */}
            <div className="bg-white shadow-lg rounded-lg">
              <div className="px-4 py-5 sm:p-6">
                <h3 className="text-lg font-medium text-gray-900 mb-6">
                  Account Actions
                </h3>
                
                <div className="space-y-4">
                  {!showChangePassword ? (
                    <button
                      onClick={() => setShowChangePassword(true)}
                      className="btn-primary"
                    >
                      Change Password
                    </button>
                  ) : (
                    <ChangePassword onClose={() => setShowChangePassword(false)} />
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

export default Profile;