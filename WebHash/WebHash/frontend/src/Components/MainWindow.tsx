import React from "react";
import NavigationBar from "./NavigationBar";
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { ThemeButton } from '../Plugins/ThemeButton';
import HashForm from "./HashTab/HashForm";



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
                <div style={{ display: "flex" }}>
                    <div style={{ flex: "50%", textAlign: "center" }}>
                        <div>
                            <HashForm />
                        </div>
                        <div>
                            <ThemeButton />
                        </div>
                    </div>
                    <div style={{ flex: "50%" }}>
                    </div>
                </div>

            </>
        );
    }

}


export default MainWindow;