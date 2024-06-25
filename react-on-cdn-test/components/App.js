import Home from "./components/Home"
import Production from "./components/Production"
import About from "./components/About"

export default function App() {
    return (<>
        <div className="appBlock">
            <h1>App</h1>
            <div className="row">
                <Home/>
                <Production/>
                <About/>
            </div>
        </div>
    </>);
}