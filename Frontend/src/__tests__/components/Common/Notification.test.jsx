import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import Notification from '../../../components/Common/Notification';
import { vi } from 'vitest';

describe('Notification Component', () => {
  const message = 'Test notification message';
  const mockOnClose = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  test('renders info notification with correct styling', () => {
    render(<Notification type="info" message={message} onClose={mockOnClose} />);
    
    // Find the notification message
    const notification = screen.getByText(message);
    expect(notification).toBeInTheDocument();
    
    // Check that the parent container has the correct classes
    const container = notification.closest('div[class*="border-l-4"]');
    expect(container).toHaveClass('bg-blue-100');
    expect(container).toHaveClass('border-blue-400');
    expect(container).toHaveClass('text-blue-700');
  });

  test('calls onClose when close button is clicked', () => {
    render(<Notification type="info" message={message} onClose={mockOnClose} />);
    
    // Find the close button
    const closeButton = screen.getByRole('button');
    fireEvent.click(closeButton);
    
    expect(mockOnClose).toHaveBeenCalled();
  });
});