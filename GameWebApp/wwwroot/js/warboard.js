(function (global) {
    const NS = {
        containers: new Map(),
        init(containerId, dotNetRef, options = {}) {
            const container = document.getElementById(containerId);
            if (!container) throw new Error('Container not found:' + containerId);
            container.innerHTML = '';
            container.classList.add('warboard-root');
            const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
            svg.setAttribute('viewBox', '0 0 1200 700');
            svg.classList.add('warboard-svg');
            const defs = document.createElementNS('http://www.w3.org/2000/svg', 'defs');
            const filter = document.createElementNS('http://www.w3.org/2000/svg', 'filter');
            filter.setAttribute('id', 'territory-shadow');
            const fe = document.createElementNS('http://www.w3.org/2000/svg', 'feDropShadow');
            fe.setAttribute('dx', '0');
            fe.setAttribute('dy', '6');
            fe.setAttribute('stdDeviation', '8');
            fe.setAttribute('floodOpacity', '0.12');
            filter.appendChild(fe);
            defs.appendChild(filter);
            svg.appendChild(defs);
            const territoriesG = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            const attacksG = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            territoriesG.classList.add('territories');
            territoriesG.addEventListener('click', function (e) {

            })
            attacksG.classList.add('attacks');
            svg.appendChild(territoriesG);
            svg.appendChild(attacksG);
            container.appendChild(svg);
            const state = {
                container,
                svg,
                territoriesG,
                attacksG,
                dotNetRef,
                playerColors: options.playerColors || ['#3b82f6', '#ef4444', '#22c55e', '#f59e0b', '#8b5cf6', '#ec4899', '#06b6d4', '#eab308'],
                neutralColor: options.neutralColor || '#94a3b8',
                selected: null,
                territoriesMap: new Map()
            };
            NS.containers.set(containerId, state);
            return true;
        },
        centroid(shape) {
            let x = 0, y = 0;
            shape.forEach(p => {
                x += p.x;
                y += p.y;
            });
            return {x: x / shape.length, y: y / shape.length}
        },
        shapeToPoints(shape) {
            return shape.map(p => `${p.x},${p.y}`).join(' ')
        },
        render(containerId, territories, attacks, selected) {
            const s = NS.containers.get(containerId);
            s.selected = selected;
            s.territoriesMap.clear();
            territories.forEach(t => s.territoriesMap.set(t.id, t));
            const g = s.territoriesG;
            g.innerHTML = '';

            territories.forEach(t => {
                const onClickAction = (e) => {
                    e.stopPropagation()
                    NS.onTerritoryClicked(containerId, t.id)
                }
                const grp = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                const poly = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
                poly.setAttribute('points', NS.shapeToPoints(t.shape));
                poly.setAttribute('fill', t.owner === null ? s.neutralColor : s.playerColors[t.owner % s.playerColors.length]);
                poly.addEventListener('click', onClickAction);
                grp.appendChild(poly);

                const c = NS.centroid(t.shape);
                const lg = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                lg.setAttribute('transform', `translate(${c.x},${c.y})`);

                const cir = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
                cir.setAttribute('r', '22');
                cir.setAttribute('fill', 'rgba(0,0,0,0.35)');
                cir.addEventListener('click', onClickAction);

                const txt = document.createElementNS('http://www.w3.org/2000/svg', 'text');
                txt.setAttribute('y', '6');
                txt.setAttribute('text-anchor', 'middle');
                txt.setAttribute('fill', '#fff');
                txt.textContent = t.troops;
                txt.addEventListener('click', onClickAction);
                lg.appendChild(cir);
                lg.appendChild(txt);
                grp.appendChild(lg);
                g.appendChild(grp);
            });
            this.drawAttacks(attacks);
        },
        drawAttacks(attacks) {
            const ag = s.attacksG;
            ag.innerHTML = '';
            attacks.forEach(a => {
                const f = NS.centroid(s.territoriesMap.get(a.from).shape);
                const t = NS.centroid(s.territoriesMap.get(a.to).shape);
                const cx = (f.x + t.x) / 2, cy = (f.y + t.y) / 2 - 60;
                const p = Math.max(0, Math.min(1, a.progress));
                const px = (1 - p) * (1 - p) * f.x + 2 * (1 - p) * p * cx + p * p * t.x;
                const py = (1 - p) * (1 - p) * f.y + 2 * (1 - p) * p * cy + p * p * t.y;
                const grp = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                grp.setAttribute('transform', `translate(${px - 10},${py - 10})`);
                const c = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
                c.setAttribute('r', '12');
                c.setAttribute('fill', '#ff0000');
                grp.appendChild(c);
                ag.appendChild(grp);
            });
        },
        onTerritoryClicked(id, tid) {
            const s = NS.containers.get(id);
            if (s.dotNetRef) s.dotNetRef.invokeMethodAsync('OnTerritoryClicked', tid);
        },
        dispose(id) {
            const s = NS.containers.get(id);
            if (!s) return;
            s.container.innerHTML = '';
            NS.containers.delete(id);
        }
    };
    global.warboard = NS;
})(window);