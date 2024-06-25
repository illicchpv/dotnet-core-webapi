const {useState} = React;
console.log('React: ', React);

export default function Production() {
    const [count, setCount] = useState(0);

    return (<>
        <h2>[Production&nbsp;

            <span>
                cnt: {count}
                <button onClick={() => setCount(count + 1)}>+</button>
                <button onClick={() => setCount(count - 1)}>-</button>
            </span>

            ]</h2>
    </>);
}