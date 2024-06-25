import App from "./components/App"

const rootElement = document.getElementById("root")
const root = ReactDOM.createRoot(rootElement)
console.log('ReactDOM: ', ReactDOM);

root.render(<>
    <App/>
</>)
