/**
 * MainLayout Component
 *
 * Main application layout with sidebar and content area.
 * Uses theme-consistent spacing and background colors.
 */

import { Outlet } from 'react-router-dom';
import Sidebar from '../components/Sidebar';

const MainLayout = () => {
  return (
    <>
      <Sidebar />
      <main className="main-content">
        <Outlet />
      </main>
    </>
  );
};

export default MainLayout;
