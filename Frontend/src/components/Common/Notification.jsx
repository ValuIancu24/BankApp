// Notification component for displaying alerts and messages
import React from 'react';

// This component provides a flexible notification system for our app
const Notification = ({ type = 'info', message, onClose }) => {
  // Define styles based on notification type
  const styles = {
    info: 'bg-blue-100 border-blue-400 text-blue-700',
    success: 'bg-green-100 border-green-400 text-green-700',
    warning: 'bg-yellow-100 border-yellow-400 text-yellow-700',
    error: 'bg-red-100 border-red-400 text-red-700'
  };

  return (
    <div className={`border-l-4 p-4 ${styles[type]} rounded relative`}>
      <div className="flex">
        <div className="flex-1">
          <p className="text-sm">{message}</p>
        </div>
        {onClose && (
          <div className="ml-auto pl-3">
            <button
              onClick={onClose}
              className="inline-flex text-lg font-semibold hover:opacity-75 focus:outline-none"
            >
              ×
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default Notification;