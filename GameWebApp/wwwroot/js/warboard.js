(function (global) {
    const NS = {
        containers: new Map(),
        territoryGroups: new Map(),
        playerIds: [],
        init(containerId, dotNetRef, options = {}) {
            this.playerIds = options.playerIds;
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

            const g = s.territoriesG;

            territories.forEach(t => {
                // Controlla se esiste già un gruppo per questo territorio
                let grp;
                if (!this.territoryGroups.has(t.id)) {
                    // crea nuovo gruppo
                    grp = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                    const onClickAction = (e) => {
                        e.stopPropagation();
                        NS.onTerritoryClicked(containerId, t);
                    }

                    // polygon principale
                    const poly = document.createElementNS('http://www.w3.org/2000/svg', 'polygon');
                    poly.setAttribute('points', NS.shapeToPoints(t.shape));
                    poly.addEventListener('click', onClickAction);
                    let color;
                    if (t.ownerId === undefined || t.ownerId === null) {
                        color = s.neutralColor
                    } else {
                        color = s.playerColors.at(this.playerIds.indexOf(t.ownerId));
                    }

                    poly.style.fill = color;
                    grp.appendChild(poly);

                    // centroide + etichetta truppe
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

                    // salva il riferimento per i prossimi render
                    this.territoryGroups.set(t.id, grp);
                    s.territoriesMap.set(t.id, t);
                    
                    return;
                }
                grp = this.territoryGroups.get(t.id);

                // aggiorna proprietà esistenti
                const poly = grp.querySelector('polygon');
                let color;
                if (t.ownerId === undefined || t.ownerId === null) {
                    color = s.neutralColor
                } else {
                    color = s.playerColors.at(this.playerIds.indexOf(t.ownerId));
                }
                poly.setAttribute('fill', color);

                const txt = grp.querySelector('text');
                txt.textContent = t.troops;

                const lg = grp.querySelector('g');
                const c = NS.centroid(t.shape);
                lg.setAttribute('transform', `translate(${c.x},${c.y})`);
            });

            // disegna gli attacchi
            this.drawAttacks(attacks, s);
        },
        drawAttacks(attacks, rootElement) {
            const ag = rootElement.attacksG;
            ag.innerHTML = '';
            attacks.forEach(a => {
                const attackerTerritory = rootElement.territoriesMap.get(a.attackerTerritoryId)
                const f = NS.centroid(attackerTerritory.shape);
                const t = NS.centroid(rootElement.territoriesMap.get(a.defenderTerritoryId).shape);
                const cx = (f.x + t.x) / 2, cy = (f.y + t.y) / 2 - 60;
                const p = Math.max(0, Math.min(1, a.progress));
                const px = (1 - p) * (1 - p) * f.x + 2 * (1 - p) * p * cx + p * p * t.x;
                const py = (1 - p) * (1 - p) * f.y + 2 * (1 - p) * p * cy + p * p * t.y;
                const grp = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                grp.setAttribute('transform', `translate(${px - 10},${py - 10})`);
                const c = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
                c.setAttribute('r', '12');
                c.setAttribute('fill', rootElement.playerColors.at(this.playerIds.indexOf(attackerTerritory.ownerId)));
                grp.appendChild(c);
                ag.appendChild(grp);
            });
        },
        onTerritoryClicked(id, territory) {
            const s = NS.containers.get(id);
            if (s.dotNetRef) s.dotNetRef.invokeMethodAsync('OnTerritoryClicked', territory);
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