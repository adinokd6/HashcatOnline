import React from 'react';
import ReactDOM from 'react-dom/client';
import MainWindow from './Components/MainWindow';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
        <MainWindow test="start" />
  </React.StrictMode>
);
