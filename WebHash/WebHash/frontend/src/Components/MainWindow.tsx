import React from "react";
import NavigationBar from "./NavigationBar";
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { ThemeButton } from '../Plugins/ThemeButton';
import HashForm from "./HashForm";


interface MainWindowProps {
    test: string
}

interface MainWindowState {
    theme: string,
}


class MainWindow extends React.Component<MainWindowProps, MainWindowState> {
    constructor(props: MainWindowProps) {
        super(props);
        this.state = {
            theme: 'light'
        }
    }



    themeToggler = () => {
        this.state.theme === 'light' ? this.setState({ theme: 'dark' }) : this.setState({ theme: 'light' })
    }


    render() {

        return (
            <>
                <NavigationBar />
                <div>
                    <HashForm />
                </div>
                <div>
                    <ThemeButton />
                </div>
            </>
        );
    }

}


export default MainWindow;